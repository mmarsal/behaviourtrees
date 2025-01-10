using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal; // Referenz zum verknüpften Portal

    private static float lastUseTime = 0f; // Zeitpunkt der letzten Verwendung aller Portale

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Alien"))
        {
            // Wenn das Portal noch nicht verwendet wurde oder die Abkühlzeit abgelaufen ist, teleportiere das Objekt
            if (lastUseTime == 0f || Time.time - lastUseTime > 10f)
            {
                Vector3 exitPosition = linkedPortal.transform.position + linkedPortal.transform.right * 2f; // Berechne die Ausgangsposition
                other.transform.position = exitPosition;

                if (other.CompareTag("Player") && PortalTracker.Instance != null)
                {
                    PortalTracker.Instance.PlayerHasTeleported(this.gameObject);
                }
                lastUseTime = Time.time;
            }
        }
    }
}