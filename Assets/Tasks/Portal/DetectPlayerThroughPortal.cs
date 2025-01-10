using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DetectPlayerThroughPortal : Conditional
{
    public SharedBool IsPlayerThroughPortal;
    public SharedGameObject Player;
    public SharedGameObject LastUsedPortal;

    public override void OnStart()
    {
        if (IsPlayerThroughPortal == null)
        {            
            IsPlayerThroughPortal = new SharedBool();
            IsPlayerThroughPortal.Value = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (PortalTracker.Instance.PlayerTeleported)
        {
            IsPlayerThroughPortal.Value = true;
            LastUsedPortal.Value = PortalTracker.Instance.LastUsedPortal;
            PortalTracker.Instance.ResetTeleportFlag();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}