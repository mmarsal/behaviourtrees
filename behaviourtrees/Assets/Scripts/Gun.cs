using UnityEngine;

public class Gun : MonoBehaviour
{
    private float damage = 10f;
    private float range = 100f;

    public Camera PlayerCam;

    [Header("Shooting Effects")]
    public ParticleSystem shootEffect; // Partikeleffekt für den Schuss
    public AudioClip shootSound; // Soundclip für den Schuss
    private AudioSource audioSource; // Audioquelle zum Abspielen des Sounds
    public Transform gunTip;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Visuelle Effekte
            if (shootEffect != null)
            {
                ParticleSystem effect = Instantiate(shootEffect, gunTip.position, gunTip.rotation);
                effect.Play(); // Stelle sicher, dass der Effekt abgespielt wird
                Destroy(effect.gameObject, effect.main.duration); // Zerstöre den Effekt nach seiner Dauer
            }

            // Soundeffekt
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }
}