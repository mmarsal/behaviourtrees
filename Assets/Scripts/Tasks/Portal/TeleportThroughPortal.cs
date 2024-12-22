using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TeleportThroughPortal : Action
{
    public SharedGameObject PortalA;
    public SharedGameObject PortalB;

    public override TaskStatus OnUpdate()
    {
        if (PortalA == null || PortalB == null)
        {
            Debug.LogWarning("Portale sind nicht zugewiesen.");
            return TaskStatus.Failure;
        }
        
        Portal portalComponent = PortalA.Value.GetComponent<Portal>();
        if (portalComponent != null && portalComponent.linkedPortal != null)
        {
            Vector3 exitPosition = portalComponent.linkedPortal.transform.position + portalComponent.linkedPortal.transform.forward * 2f;
            this.transform.position = exitPosition;
            Debug.Log("Alien teleportiert zum verknüpften Portal.");
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}