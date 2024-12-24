using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal; // Referenz zum verknüpften Portal

    private static float lastUseTime = 0f; // Zeitpunkt der letzten Verwendung aller Portale

    private void OnTriggerEnter(Collider other)
    {
        if (linkedPortal == null)
        {
            Debug.LogWarning("Verknüpftes Portal nicht zugewiesen.");
            return;
        }

        // Überprüfe, ob das Objekt ein Spieler oder Alien ist
        if (other.CompareTag("Player") || other.CompareTag("Alien"))
        {
            // Wenn das Portal noch nicht verwendet wurde oder die Abkühlzeit abgelaufen ist, teleportiere das Objekt
            if (lastUseTime == 0f || Time.time - lastUseTime > 10f)
            {
                Vector3 exitPosition = linkedPortal.transform.position + linkedPortal.transform.right * 2f; // Offset, um direkt vor dem Portal zu erscheinen
                other.transform.position = exitPosition;

                // Informiere den PortalTracker, wenn es der Spieler ist
                if (other.CompareTag("Player") && PortalTracker.Instance != null)
                {
                    PortalTracker.Instance.PlayerHasTeleported(this.gameObject); // Pass das aktuell verwendete Portal
                }

                // Setze die letzte Verwendung auf die aktuelle Zeit
                lastUseTime = Time.time;
            }
        }
    }
}