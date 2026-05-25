using System.Collections;
using UnityEngine;
using TMPro;

public class ItemAcquiredPopup : MonoBehaviour
{
    public GameObject popupObject;
    public TMP_Text popupText;

    [Header("Timing")]
    public float fadeInTime = 0.3f;
    public float displayTime = 1.5f;
    public float fadeOutTime = 0.6f;

    private Coroutine currentPopup;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (popupObject != null)
        {
            canvasGroup = popupObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = popupObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            popupObject.SetActive(false);
        }
    }

    public void ShowPopup(string message)
    {
        if (currentPopup != null)
        {
            StopCoroutine(currentPopup);
        }

        currentPopup = StartCoroutine(ShowPopupRoutine(message));
    }

    private IEnumerator ShowPopupRoutine(string message)
    {
        if (popupObject == null || popupText == null)
        {
            yield break;
        }

        popupText.text = message;
        popupObject.SetActive(true);

        // Fade in
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Stay visible
        yield return new WaitForSeconds(displayTime);

        // Fade out
        timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        popupObject.SetActive(false);
    }
}