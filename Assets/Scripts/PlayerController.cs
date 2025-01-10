using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Spieler sieht schwarz
    public Image blackoutImage;
    private bool isBlackout = false;

    public void TriggerBlackout(float duration)
    {
        if (!isBlackout)
        {
            StartCoroutine(BlackoutRoutine(duration));
        }
    }

    private IEnumerator BlackoutRoutine(float duration)
    {
        isBlackout = true;
        blackoutImage.color = new Color(0, 0, 0, 0.96f);
        blackoutImage.enabled = true;
        yield return new WaitForSeconds(duration);
        blackoutImage.enabled = false;
        isBlackout = false;
    }
}