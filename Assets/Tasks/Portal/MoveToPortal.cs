using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPortal : Action
{
    public SharedGameObject LastUsedPortal;
    public SharedBool UsePortal;
    public SharedGameObject CurrentPortal;

    private NavMeshAgent agent;
    public float arrivalTolerance = 2f; // Toleranz für Ankunft am Portal

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();

        if (UsePortal.Value)
        {
            CurrentPortal.Value = LastUsedPortal.Value;
        }
        else
        {
            Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
            if (portalComponent != null && portalComponent.linkedPortal != null)
            {
                CurrentPortal.Value = portalComponent.linkedPortal.gameObject;
                Debug.Log($"MoveToPortal: Verknüpftes Portal: {CurrentPortal.Value.name}");
            }
        }

        if (CurrentPortal.Value != null)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(CurrentPortal.Value.transform.position, path);
            agent.SetPath(path);
        }
    }

    public override TaskStatus OnUpdate()
    {
        float distanceToPortal = Vector3.Distance(transform.position, CurrentPortal.Value.transform.position);

        if (distanceToPortal <= arrivalTolerance)
        {
            return TaskStatus.Success;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.ResetPath();
                    return TaskStatus.Success;
                }
            }
        }
        
        return TaskStatus.Running;
    }
}