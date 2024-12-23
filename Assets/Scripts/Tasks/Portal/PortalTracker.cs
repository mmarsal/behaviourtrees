using UnityEngine;

public class PortalTracker : MonoBehaviour
{
    public static PortalTracker Instance;

    public bool PlayerTeleported = false;
    public GameObject LastUsedPortal = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerHasTeleported(GameObject portal)
    {
        Debug.Log("Player has teleported through a portal.");
        PlayerTeleported = true;
        LastUsedPortal = portal;
    }

    public void ResetTeleportFlag()
    {
        Debug.Log("Resetting teleport flag.");
        PlayerTeleported = false;
        LastUsedPortal = null;
    }
}