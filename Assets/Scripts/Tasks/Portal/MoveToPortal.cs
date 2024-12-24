using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPortal : Action
{
    public SharedGameObject LastUsedPortal; // Letztes verwendetes Portal
    public SharedBool UsePortal; // Entscheidung ob Portal benutzt werden soll
    public SharedGameObject CurrentPortal;   // Zielportal, zu dem das Alien laufen soll

    private NavMeshAgent agent;
    public float arrivalTolerance = 2f; // Toleranz für Ankunft am Portal

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

        // Bestimme das Zielportal basierend auf UsePortal
        if (UsePortal.Value)
        {
            // Move to LastUsedPortal (source portal)
            CurrentPortal.Value = LastUsedPortal.Value;
            Debug.Log($"MoveToPortal: Moving to LastUsedPortal: {CurrentPortal.Value.name}");
        }
        else
        {
            // Move to linked portal (target portal)
            Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
            if (portalComponent != null && portalComponent.linkedPortal != null)
            {
                CurrentPortal.Value = portalComponent.linkedPortal.gameObject;
                Debug.Log($"MoveToPortal: Moving to linked Portal: {CurrentPortal.Value.name}");
            }
            else
            {
                Debug.LogWarning("Verknüpftes Portal nicht gefunden.");
                CurrentPortal.Value = null;
            }
        }

        if (CurrentPortal.Value != null)
        {
            agent.SetDestination(CurrentPortal.Value.transform.position);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (agent == null || CurrentPortal == null || CurrentPortal.Value == null)
        {
            Debug.LogWarning("NavMeshAgent oder CurrentPortal ist nicht zugewiesen.");
            return TaskStatus.Failure;
        }

        float distanceToPortal = Vector3.Distance(transform.position, CurrentPortal.Value.transform.position);
        Debug.Log($"Alien ist {distanceToPortal} Einheiten vom Portal entfernt.");

        if (distanceToPortal <= arrivalTolerance)
        {
            Debug.Log("Alien hat das Portal innerhalb der Toleranz erreicht.");
            return TaskStatus.Success;
        }

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
