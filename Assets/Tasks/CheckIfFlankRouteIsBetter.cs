using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CheckIfFlankRouteIsBetter : Action
{
    [Header("Referenzen")]
    public SharedGameObject player;
    public SharedGameObject alien;
    public SharedGameObjectList flankPoints;

    [Header("Optionen")]
    public float tolerance = 1.0f; // Abstandstoleranz für das Erreichen des Ziels

    private NavMeshAgent navAgent;
    private GameObject currentFlankPoint;

    public override void OnStart()
    {
        base.OnStart();
        navAgent = alien.Value.GetComponent<NavMeshAgent>();
    }

    private GameObject GetClosestFlankPoint(Vector3 alienPosition)
    {
        GameObject closestPoint = null;
        float closestDistance = float.MaxValue;

        foreach (var point in flankPoints.Value)
        {
            if (point == null)
                continue;

            float distance = Vector3.Distance(alienPosition, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    public override TaskStatus OnUpdate()
    {
        GameObject closestFlankPoint = GetClosestFlankPoint(alien.Value.transform.position);
        float flankDistance = Vector3.Distance(alien.Value.transform.position, closestFlankPoint.transform.position);
        float playerDistance = Vector3.Distance(alien.Value.transform.position, player.Value.transform.position);

        if (flankDistance < playerDistance)
        {
            // Flankenpunkt als Ziel setzen, falls noch nicht gesetzt oder sich geändert hat
            if (currentFlankPoint != closestFlankPoint)
            {
                bool destinationSet = navAgent.SetDestination(closestFlankPoint.transform.position);
                if (destinationSet)
                {
                    currentFlankPoint = closestFlankPoint;
                    Debug.Log("Bewege zu Flankpunkt");
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }

            if (navAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                navAgent.ResetPath();
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        else
        {
            // Flankenpunkt ist nicht näher, keine Aktion ausführen
            Debug.Log("Flankpunkt ist nicht näher als der Spieler. Keine Aktion ausgeführt.");
            return TaskStatus.Failure;
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();
        if (navAgent != null && !navAgent.pathPending)
        {
            navAgent.ResetPath();
        }
    }
}
