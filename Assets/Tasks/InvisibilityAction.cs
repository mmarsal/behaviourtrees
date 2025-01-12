using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InvisibilityAction : Action
{
    public float invisibilityDuration = 5f;
    public float cooldownTime = 10f;
    
    [Header("References")]
    public SharedTransform playerTransform;
    public Renderer alienRenderer;
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip invisibilitySound;
    public AudioClip visibilitySound;

    [Header("Optional: Proximity Sound")]
    public bool useProximitySound = false;
    public AudioClip hummingSound;
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
        hummingSource = gameObject.AddComponent<AudioSource>();
        hummingSource.clip = hummingSound;
        hummingSource.loop = true;
        hummingSource.volume = 0f;
        hummingSource.playOnAwake = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (cooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownActive = false;
            }
        }
        
        if (isInvisible)
        {
            invisibilityTimer -= Time.deltaTime;
            // Spielen von Summen wenn True ist
            if (useProximitySound && hummingSource.isPlaying)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.Value.position);
                float t = Mathf.InverseLerp(maxDistance, 0f, distance);
                float volume = Mathf.Lerp(minHummingVolume, maxHummingVolume, t);
                hummingSource.volume = volume;
            }

            if (invisibilityTimer <= 0f)
            {
                SetVisible(true);
                isInvisible = false;
                audioSource.PlayOneShot(visibilitySound);

                if (useProximitySound && hummingSource.isPlaying)
                {
                    hummingSource.Stop();
                }

                cooldownActive = true;
                cooldownTimer = cooldownTime;

                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        else
        {
            if (!cooldownActive)
            {
                BecomeInvisible();
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }

    private void BecomeInvisible()
    {
        isInvisible = true;
        invisibilityTimer = invisibilityDuration;
        SetVisible(false);
        audioSource.PlayOneShot(invisibilitySound);

        if (useProximitySound && !hummingSource.isPlaying)
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
