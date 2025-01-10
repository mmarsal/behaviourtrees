using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UpdatePlayerPosition : Action
{
    public SharedVector3 ApproxPlayerPosition;
    public float updateInterval = 30f;

    private float timer;
    private Transform playerTransform;

    public override void OnStart()
    {
        timer = updateInterval;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override TaskStatus OnUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Vector3 exactPosition = playerTransform.position;
            
            float offsetRange = 2f; // Maximaler Offset in jeder Richtung, für Randomizer
            Vector3 approximatePosition = exactPosition + new Vector3(
                Random.Range(-offsetRange, offsetRange),
                0f,
                Random.Range(-offsetRange, offsetRange)
            );

            ApproxPlayerPosition.Value = approximatePosition;
            timer = updateInterval;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}