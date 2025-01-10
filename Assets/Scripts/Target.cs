using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class Target : MonoBehaviour
{
    // ALien haut ab falls er getroffen wird
    public float health = 1f;
    public SharedTransform playerTransform;
    private BehaviorTree behaviorTree;
    public GameObject doppelganger;

    private void Start()
    {
        playerTransform.Value = GameObject.FindWithTag("Player").transform;
        behaviorTree = GetComponent<BehaviorTree>();
    }

    private void Update()
    {
        if (health <= 1f)
        {
            behaviorTree.SendEvent<object>("Flee", 5);
            RestoreHealth();
            Invoke(nameof(RestoreHealth), 3f);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    public void RestoreHealth()
    {
        health = 100f;
        doppelganger.SetActive(false);
    }
}
