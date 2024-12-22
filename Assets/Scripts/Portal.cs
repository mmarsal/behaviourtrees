using UnityEngine;
using System;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public static event Action<GameObject> OnTeleported;

    private void OnTriggerEnter(Collider other)
    {
        if (linkedPortal == null)
        {
            Debug.LogWarning("Verknüpftes Portal nicht zugewiesen.");
            return;
        }

        if (other.CompareTag("Player") || other.CompareTag("Alien"))
        {
            Vector3 exitPosition = linkedPortal.transform.position + linkedPortal.transform.forward * 2f; // Offset, um direkt vor dem Portal zu erscheinen
            other.transform.position = exitPosition;

            OnTeleported?.Invoke(other.gameObject);
        }
    }
}