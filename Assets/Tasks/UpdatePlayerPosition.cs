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

        // Finde den Spieler anhand des Tags "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Kein GameObject mit dem Tag 'Player' gefunden.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Spieler-Transform ist nicht zugewiesen.");
            return TaskStatus.Failure;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Aktualisiere die ungefähre Position des Spielers
            Vector3 exactPosition = playerTransform.position;

            // Simuliere eine ungefähre Position (z.B. mit zufälligem Offset)
            float offsetRange = 2f; // Maximaler Offset in jeder Richtung
            Vector3 approximatePosition = exactPosition + new Vector3(
                Random.Range(-offsetRange, offsetRange),
                0f,
                Random.Range(-offsetRange, offsetRange)
            );

            ApproxPlayerPosition.Value = approximatePosition;
            Debug.Log($"Spielerposition aktualisiert: {approximatePosition}");

            // Reset des Timers
            timer = updateInterval;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}