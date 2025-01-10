using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ChooseBestAction : Action
{
    [Header("Referenzen")]
    public SharedBool UsePortal;
    public SharedGameObject LastUsedPortal;
    public SharedGameObject PortalA;
    public SharedGameObject PortalB;
    public SharedGameObject Player;
    public SharedGameObject Alien;

    [Header("Teleportation Einstellungen")]
    public float teleportThreshold = 15f;
    public float teleportCooldown = 5f;

    private float lastTeleportTime = -Mathf.Infinity;

    public override void OnStart()
    {
        base.OnStart();

        if (UsePortal == null)
        {            
            UsePortal = new SharedBool();
            UsePortal.Value = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        // Bestimme verknüpfte Portal
        Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
        GameObject targetPortal = portalComponent.linkedPortal.gameObject;

        // Berechnen Distanz zum Zielportal vom Alien
        float distanceToTargetPortal = Vector3.Distance(Alien.Value.transform.position, targetPortal.transform.position);
        
        // Entscheiden ob teleportieren schneller ist
        if (distanceToTargetPortal < teleportThreshold && Time.time > lastTeleportTime + teleportCooldown)
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
