using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPortal : Action
{
    public SharedGameObject CurrentPortal;

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