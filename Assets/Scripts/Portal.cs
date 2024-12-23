using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal; // Referenz zum verknüpften Portal

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
            // Teleportiere das Objekt zum verknüpften Portal
            Vector3 exitPosition = linkedPortal.transform.position + linkedPortal.transform.forward * 2f; // Offset, um direkt vor dem Portal zu erscheinen
            other.transform.position = exitPosition;

            // Informiere den PortalTracker, wenn es der Spieler ist
            if (other.CompareTag("Player") && PortalTracker.Instance != null)
            {
                PortalTracker.Instance.PlayerHasTeleported(this.gameObject); // Pass das aktuell verwendete Portal
            }
        }
    }
}