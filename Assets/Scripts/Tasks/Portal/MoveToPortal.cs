using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPortal : Action
{
    public SharedGameObject LastUsedPortal; // Referenz zum zuletzt verwendeten Portal
    public SharedGameObject CurrentPortal;   // Zielportal, zu dem das Alien laufen soll

    private NavMeshAgent agent;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent nicht gefunden.");
            return;
        }

        if (LastUsedPortal == null || LastUsedPortal.Value == null)
        {
            Debug.LogWarning("LastUsedPortal ist nicht zugewiesen.");
            return;
        }

        // Setze CurrentPortal auf das verknüpfte Portal des LastUsedPortal
        Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
        if (portalComponent != null && portalComponent.linkedPortal != null)
        {
            CurrentPortal.Value = portalComponent.linkedPortal.gameObject;
            Debug.Log($"CurrentPortal gesetzt auf: {CurrentPortal.Value.name}");
        }
        else
        {
            Debug.LogWarning("Verknüpftes Portal nicht gefunden.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (CurrentPortal == null || CurrentPortal.Value == null)
        {
            Debug.LogWarning("CurrentPortal ist nicht zugewiesen.");
            return TaskStatus.Failure;
        }

        agent.SetDestination(CurrentPortal.Value.transform.position);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Alien hat das Portal erreicht.");
                    return TaskStatus.Success;
                }
            }
        }

        return TaskStatus.Running;
    }
}