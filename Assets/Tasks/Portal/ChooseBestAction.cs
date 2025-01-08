using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ChooseBestAction : Action
{
    [Header("Referenzen")]
    public SharedBool UsePortal;
    public SharedGameObject LastUsedPortal; // Letztes verwendetes Portal
    public SharedGameObject PortalA;
    public SharedGameObject PortalB;
    public SharedGameObject Player;
    public SharedGameObject Alien; // Referenz zum Alien-GameObject

    [Header("Teleportation Einstellungen")]
    public float teleportThreshold = 15f; // Schwellenwert für Teleportation
    public float teleportCooldown = 5f; // Cooldown zwischen Teleportationen

    private float lastTeleportTime = -Mathf.Infinity;

    public override void OnStart()
    {
        base.OnStart();

        if (UsePortal == null)
        {            
            UsePortal = new SharedBool();
            UsePortal.Value = false;
        }

        if (Alien == null || Alien.Value == null)
        {
            Debug.LogError("Alien-Referenz ist nicht zugewiesen oder null.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (Alien == null || Alien.Value == null)
        {
            Debug.LogWarning("Alien-Objekt ist null.");
            UsePortal.Value = false;
            return TaskStatus.Failure;
        }

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

        // Berechne die Distanz zum Zielportal vom Alien
        float distanceToTargetPortal = Vector3.Distance(Alien.Value.transform.position, targetPortal.transform.position);

        Debug.Log($"Alien berechnet die Distanz zum Portal: {distanceToTargetPortal}");

        // Entscheide, ob teleportieren schneller ist
        if (distanceToTargetPortal < teleportThreshold && Time.time > lastTeleportTime + teleportCooldown)
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
