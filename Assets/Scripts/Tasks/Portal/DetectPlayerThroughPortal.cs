using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DetectPlayerThroughPortal : Conditional
{
    public SharedBool IsPlayerThroughPortal;
    public SharedGameObject Player; // Spieler als Shared Variable
    public SharedGameObject LastUsedPortal; // Letztes verwendetes Portal

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
        }

        // Starte ohne Teleportation
        PortalTracker.Instance.ResetTeleportFlag();
    }

    public override TaskStatus OnUpdate()
    {
        if (PortalTracker.Instance.PlayerTeleported)
        {
            IsPlayerThroughPortal.Value = true;
            LastUsedPortal.Value = PortalTracker.Instance.LastUsedPortal;
            PortalTracker.Instance.ResetTeleportFlag();
            Debug.Log($"Player wurde durch ein Portal teleportiert: {LastUsedPortal.Value.name}");
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}