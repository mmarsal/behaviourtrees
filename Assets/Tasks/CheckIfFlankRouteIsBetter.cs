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

        if (alien.Value != null)
        {
            navAgent = alien.Value.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("NavMeshAgent-Komponente fehlt am Alien-Objekt.");
            }
        }
        else
        {
            Debug.LogError("Alien-Objekt ist null.");
        }

        // Sicherstellen, dass flankPoints nicht null ist
        if (flankPoints == null || flankPoints.Value == null)
        {
            Debug.LogError("flankPoints ist nicht initialisiert.");
        }
    }

    private GameObject GetClosestFlankPoint(Vector3 alienPosition)
    {
        if (flankPoints == null || flankPoints.Value == null || flankPoints.Value.Count == 0)
        {
            Debug.LogWarning("flankPoints ist leer oder null.");
            return null;
        }

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
        if (player.Value == null || alien.Value == null)
        {
            Debug.LogWarning("Player oder Alien ist null.");
            return TaskStatus.Failure;
        }

        if (navAgent == null)
        {
            Debug.LogWarning("NavMeshAgent ist nicht initialisiert.");
            return TaskStatus.Failure;
        }

        // Nächstgelegenen Flankenpunkt ermitteln
        GameObject closestFlankPoint = GetClosestFlankPoint(alien.Value.transform.position);
        if (closestFlankPoint == null)
        {
            return TaskStatus.Failure;
        }

        // Entfernung zum Flankenpunkt berechnen
        float flankDistance = Vector3.Distance(alien.Value.transform.position, closestFlankPoint.transform.position);

        // Entfernung zum Spieler berechnen
        float playerDistance = Vector3.Distance(alien.Value.transform.position, player.Value.transform.position);

        // Überprüfen, ob der Flankenpunkt näher ist als der Spieler
        if (flankDistance < playerDistance)
        {
            // Flankenpunkt als Ziel setzen, falls noch nicht gesetzt oder sich geändert hat
            if (currentFlankPoint != closestFlankPoint)
            {
                navAgent.SetDestination(closestFlankPoint.transform.position);
                currentFlankPoint = closestFlankPoint;
                Debug.Log("Bewege zu Flankpunkt: " + closestFlankPoint.name);
            }

            // Überprüfen, ob das Ziel erreicht wurde
            if (!navAgent.pathPending)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance + tolerance)
                {
                    Debug.Log("Flankpunkt erreicht: " + closestFlankPoint.name);
                    return TaskStatus.Success;
                }
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
        // Optional: Aktion beim Beenden des Tasks, z.B. NavMeshAgent stoppen
        if (navAgent != null && !navAgent.pathPending)
        {
            navAgent.ResetPath();
        }
    }
}
