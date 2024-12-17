using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InvisibilityAction : Action
{
    public float invisibilityDuration = 5f;  // Wie lange bleibt der Alien unsichtbar?
    public float cooldownTime = 10f;         // Wie lange muss gewartet werden, bis der Alien wieder unsichtbar werden kann?

    [Header("References")]
    public SharedTransform playerTransform;  // Referenz auf den Player
    public Renderer alienRenderer;           // Der Renderer des Aliens.
    public AudioSource audioSource;          // AudioSource für die Sounds

    [Header("Sounds")]
    public AudioClip invisibilitySound;      // Sound beim Unsichtbarwerden
    public AudioClip visibilitySound;        // Sound beim Wieder-sichtbarwerden

    [Header("Optional: Proximity Sound")]
    public bool useProximitySound = false;
    public AudioClip hummingSound;           // Dauerton
    public float maxHummingVolume = 1f;
    public float minHummingVolume = 0.1f;
    public float maxDistance = 20f;

    private bool isInvisible = false;
    private float invisibilityTimer = 0f;
    private float cooldownTimer = 0f;
    private bool cooldownActive = false;

    private AudioSource hummingSource;

    public override void OnStart()
    {
        // Falls wir den hummingSound nutzen, erstellen wir dafür eine separate AudioSource
        if (useProximitySound && hummingSound != null && playerTransform.Value != null)
        {
            hummingSource = gameObject.AddComponent<AudioSource>();
            hummingSource.clip = hummingSound;
            hummingSource.loop = true;
            hummingSource.volume = 0f;
            hummingSource.playOnAwake = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        // Cooldown herunterzählen
        if (cooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownActive = false;
            }
        }

        // Wenn bereits unsichtbar, zähle die Zeit runter
        if (isInvisible)
        {
            invisibilityTimer -= Time.deltaTime;

            // Optional: Lautstärke der hummingSource anpassen
            if (useProximitySound && hummingSource != null && hummingSource.isPlaying)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.Value.position);
                float t = Mathf.InverseLerp(maxDistance, 0f, distance);
                float volume = Mathf.Lerp(minHummingVolume, maxHummingVolume, t);
                hummingSource.volume = volume;
            }

            // Wenn Unsichtbarkeitsdauer abgelaufen ist, wieder sichtbar werden
            if (invisibilityTimer <= 0f)
            {
                SetVisible(true);
                isInvisible = false;
                if (visibilitySound != null)
                {
                    audioSource.PlayOneShot(visibilitySound);
                }

                // Humming stoppen
                if (useProximitySound && hummingSource != null && hummingSource.isPlaying)
                {
                    hummingSource.Stop();
                }

                // Cooldown starten
                cooldownActive = true;
                cooldownTimer = cooldownTime;

                // Jetzt ist der Task fertig -> Success zurückgeben, damit der nächste Node ausgeführt wird
                return TaskStatus.Success;
            }

            // Noch unsichtbar -> Task läuft weiter
            return TaskStatus.Running;
        }
        else
        {
            // Noch nicht unsichtbar und Player ist bereits gesichtet (durch den vorherigen Task)
            // Prüfen, ob wir Unsichtbarkeit starten können
            if (!cooldownActive)
            {
                // Unsichtbar werden
                BecomeInvisible();
                return TaskStatus.Running; // Wir müssen warten, bis die Dauer abläuft.
            }
            else
            {
                // Cooldown aktiv, wir können nicht unsichtbar werden -> Mache Failure
                // Dadurch geht die Sequence nicht weiter. Wenn du möchtest, dass die Sequence dennoch weitermacht, gib hier Success.
                return TaskStatus.Failure;
            }
        }
    }

    private void BecomeInvisible()
    {
        isInvisible = true;
        invisibilityTimer = invisibilityDuration;
        SetVisible(false);
        if (invisibilitySound != null)
        {
            audioSource.PlayOneShot(invisibilitySound);
        }

        // Humming starten
        if (useProximitySound && hummingSource != null && !hummingSource.isPlaying)
        {
            hummingSource.Play();
        }
    }

    private void SetVisible(bool visible)
    {
        if (alienRenderer != null)
        {
            alienRenderer.enabled = visible;
        }
    }
}
