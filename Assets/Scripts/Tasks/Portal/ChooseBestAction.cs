using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ChooseBestAction : Action
{
    public SharedBool UsePortal;
    public SharedGameObject PortalA;
    public SharedGameObject PortalB;
    public SharedGameObject Player;
    public float teleportCooldown = 5f;

    private float lastTeleportTime = -Mathf.Infinity;

    public override TaskStatus OnUpdate()
    {
        float distanceToPortalA = Vector3.Distance(Player.Value.transform.position, PortalA.Value.transform.position);
        float distanceToPortalB = Vector3.Distance(Player.Value.transform.position, PortalB.Value.transform.position);

        GameObject nearestPortal = distanceToPortalA < distanceToPortalB ? PortalA.Value : PortalB.Value;

        float distanceToNearestPortal = Vector3.Distance(this.transform.position, nearestPortal.transform.position);
        float timeToWalk = distanceToNearestPortal / GetComponent<UnityEngine.AI.NavMeshAgent>().speed;

        float teleportTime = 1f;

        if (teleportTime < timeToWalk && Time.time > lastTeleportTime + teleportCooldown)
        {
            UsePortal.Value = true;
            lastTeleportTime = Time.time;
        }
        else
        {
            UsePortal.Value = false;
        }

        return TaskStatus.Success;
    }
}