using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class TeleportThroughPortal : Action
{
    public SharedGameObject LastUsedPortal; // Letztes verwendetes Portal
    public float teleportRange = 2f; // Nähe zum Portal, bevor teleportiert wird

    private NavMeshAgent agent;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {

        Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();

        Vector3 portalPosition = LastUsedPortal.Value.transform.position;
        float distanceToPortal = Vector3.Distance(transform.position, portalPosition);

        if (distanceToPortal <= teleportRange)
        {
            // Teleportiere das Alien zum verknüpften Portal
            Vector3 exitPosition = portalComponent.linkedPortal.transform.position + portalComponent.linkedPortal.transform.forward * 2f; // Offset, um direkt vor dem Portal zu erscheinen

            if (agent != null)
            {
                agent.Warp(exitPosition); // Warping ist besser mit NavMeshAgent
            }
            else
            {
                transform.position = exitPosition;
            }


            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
