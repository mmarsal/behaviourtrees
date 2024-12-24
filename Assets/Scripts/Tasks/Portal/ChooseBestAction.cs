using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ChooseBestAction : Action
{
    public SharedBool UsePortal;
    public SharedGameObject LastUsedPortal; // Letztes verwendetes Portal
    public SharedGameObject PortalA;
    public SharedGameObject PortalB;
    public SharedGameObject Player;

    private float lastTeleportTime = -Mathf.Infinity;

    public override void OnStart()
    {
        if (UsePortal == null)
        {            
            UsePortal = new SharedBool();
            UsePortal.Value = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (LastUsedPortal == null || LastUsedPortal.Value == null)
        {
            Debug.LogWarning("LastUsedPortal ist nicht zugewiesen.");
            UsePortal.Value = false;
            return TaskStatus.Success;
        }

        // Bestimme das verknüpfte Portal
        Portal portalComponent = LastUsedPortal.Value.GetComponent<Portal>();
        if (portalComponent == null || portalComponent.linkedPortal == null)
        {
            Debug.LogWarning("Verknüpftes Portal nicht gefunden.");
            UsePortal.Value = false;
            return TaskStatus.Failure;
        }

        GameObject targetPortal = portalComponent.linkedPortal.gameObject;

        // Berechne die Distanz zum Zielportal
        float distanceToTargetPortal = Vector3.Distance(this.transform.position, targetPortal.transform.position);

        // Entscheide, ob teleportieren schneller ist
        float teleportThreshold = 20f; // Beispielwert, teleportieren wenn Portal weit entfernt ist
        
        Debug.Log($"Alien berechnet die Distanz zum Portal: {distanceToTargetPortal}");
        
        if (distanceToTargetPortal < teleportThreshold && Time.time > lastTeleportTime)
        {
            UsePortal.Value = true;
            lastTeleportTime = Time.time;
            Debug.Log($"Alien entscheidet, das Portal zu teleportieren: {targetPortal.name}");
        }
        else
        {
            UsePortal.Value = false;
            Debug.Log($"Alien entscheidet, nicht zu teleportieren. Distanz zum Portal: {distanceToTargetPortal}");
        }

        return TaskStatus.Success;
    }
}
