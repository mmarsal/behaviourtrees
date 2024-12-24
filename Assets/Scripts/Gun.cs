using UnityEngine;
using BehaviorDesigner.Runtime;

public class Gun : MonoBehaviour
{
    private float damage = 50f;
    private float range = 10f;
    private float cooldown = 2f;

    private float lastShootTime = 0f;

    public Camera PlayerCam;

    [Header("Shooting Effects")]
    public ParticleSystem shootEffect; // Partikeleffekt für den Schuss
    public AudioClip shootSound;       // Soundclip für den Schuss
    public Transform gunTip;           // Position am Gewehrlauf für den Mündungsfeuer-Effekt

    public GameObject npcGameObject;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.LeftControl) && CanShoot())
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        return Time.time - lastShootTime > cooldown;
    }

    private void Shoot()
    {
        lastShootTime = Time.time;

        RaycastHit hit;
        if (Physics.SphereCast(PlayerCam.transform.position, 1f, PlayerCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Visuelle Effekte auslösen
            if (shootEffect != null && gunTip != null)
            {
                ParticleSystem effect = Instantiate(shootEffect, gunTip.position, gunTip.rotation);
                effect.transform.parent = gunTip;
                effect.Play();
            }

            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
                BehaviorTree behaviorTree = npcGameObject.GetComponent<BehaviorTree>();

                if (behaviorTree != null)
                {
                    Vector3 noisePosition = gameObject.transform.position;
                    behaviorTree.SetVariableValue("noisePosition", noisePosition);
                    behaviorTree.SendEvent("NoiseHeardEvent");
                    Debug.LogWarning("Sound bekommen!");
                }
            }
        }
    }
}