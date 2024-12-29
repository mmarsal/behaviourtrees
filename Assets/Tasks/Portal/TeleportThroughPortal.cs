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
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent nicht gefunden.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (LastUsedPortal == null || LastUsedPortal.Value == null)
        {
            Debug.LogWarning("LastUsedPortal ist nicht zugewiesen.");
            return TaskStatus.Failure;
        }

        Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
        if (portalComponent == null || portalComponent.linkedPortal == null)
        {
            Debug.LogWarning("Verknüpftes Portal nicht gefunden.");
            return TaskStatus.Failure;
        }

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

            Debug.Log($"Alien teleportiert zum verknüpften Portal: {portalComponent.linkedPortal.name}");

            return TaskStatus.Success;
        }
        else
        {
            Debug.Log($"Alien ist noch nicht am Portal. Distanz: {distanceToPortal}");
            return TaskStatus.Failure;
        }
    }
}
