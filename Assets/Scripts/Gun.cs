using UnityEngine;
using BehaviorDesigner.Runtime;

public class Gun : MonoBehaviour
{
    private float damage = 50f;
    private float range = 100f;

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

            // Visuelle Effekte auslösen
            if (shootEffect != null && gunTip != null)
            {
                ParticleSystem effect = Instantiate(shootEffect, gunTip.position, gunTip.rotation);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration); 
            }
            
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
                BehaviorTree behaviorTree = npcGameObject.GetComponent<BehaviorTree>();

                if (behaviorTree != null)
                {
                    Vector3 noisePosition = hit.point;
                    behaviorTree.SetVariableValue("noisePosition", noisePosition);
                    behaviorTree.SendEvent("NoiseHeardEvent");
                    Debug.LogWarning("Sound bekommen!");
                }
            }
        }
    }
}