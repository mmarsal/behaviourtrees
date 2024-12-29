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
            // Optional: Verhindern, dass das Objekt beim Laden einer neuen Szene zerstört wird
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerHasTeleported(GameObject portal)
    {
        PlayerTeleported = true;
        LastUsedPortal = portal;
    }

    public void ResetTeleportFlag()
    {
        PlayerTeleported = false;
        LastUsedPortal = null;
    }
}