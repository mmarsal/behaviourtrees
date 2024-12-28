using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class GetFleePoint : Action
{
    public SharedGameObject objectA;
    public SharedGameObject objectB;

    private BehaviorTree behaviorTree;

    public override void OnStart()
    {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public override TaskStatus OnUpdate()
    {
        if (objectA.Value == null || objectB.Value == null)
            return TaskStatus.Failure;

        NavMeshPath pathA = new();
        NavMeshPath pathB = new();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Calculate paths
        agent.CalculatePath(objectA.Value.transform.position, pathA);
        agent.CalculatePath(objectB.Value.transform.position, pathB);

        // Compare path lengths
        float distanceA = GetPathLength(pathA);
        float distanceB = GetPathLength(pathB);

        // Assign the further object
        behaviorTree.SetVariableValue("furtherFleePoint", distanceA > distanceB ? objectA.Value : objectB.Value);

        return TaskStatus.Success;
    }

    private float GetPathLength(NavMeshPath path)
    {
        if (path.status != NavMeshPathStatus.PathComplete)
            return float.MaxValue;

        float length = 0;
        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }
}
