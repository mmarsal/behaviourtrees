using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SetAmbushPosition : Action
{
    public SharedGameObjectList AmbushPoints;
    public SharedBool atAmbushPosition;
    public float speed = 20f;

    private NavMeshAgent agent;
    
    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found.");
            return;
        }

        // Finde den nächsten Ambush-Point
        GameObject selectedPoint = null;
        float closestDistance = float.MaxValue;

        foreach (var point in AmbushPoints.Value)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                selectedPoint = point;
            }
        }

        agent.SetDestination(selectedPoint.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            atAmbushPosition.Value = true;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}