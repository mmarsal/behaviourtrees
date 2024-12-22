using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DetectPlayerThroughPortal : Conditional
{
    public SharedBool IsPlayerThroughPortal;
    public SharedGameObject Player;
    private bool teleported = false;

    public override void OnStart()
    {
        if (IsPlayerThroughPortal == null)
        {            
            IsPlayerThroughPortal = new SharedBool();
            IsPlayerThroughPortal.Value = false;
        }

        if (Player == null || Player.Value == null)
        {
            Debug.LogWarning("Player-Referenz ist nicht gesetzt.");
            return;
        }

        // Abonniere das Teleportations-Event
        Portal.OnTeleported += HandleTeleport;
    }

    public override void OnEnd()
    {
        // Abonniere das Teleportations-Event nicht weiter, um Speicherlecks zu vermeiden
        Portal.OnTeleported -= HandleTeleport;
    }

    private void HandleTeleport(GameObject obj)
    {
        if (obj == Player.Value)
        {
            teleported = true;
            IsPlayerThroughPortal.Value = true;
            Debug.Log("Player wurde durch ein Portal teleportiert.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (teleported)
        {
            teleported = false;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}