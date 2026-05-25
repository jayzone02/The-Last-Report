using System.Collections;
using UnityEngine;

public class LastReportInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject reportPromptText;
    public GameObject journalPanel;

    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Journal Animation")]
    public float animationDuration = 0.35f;
    public Vector2 closedPosition = new Vector2(0, -450);
    public Vector2 openPosition = new Vector2(0, 0);
    public Vector3 closedScale = new Vector3(0.25f, 0.25f, 0.25f);
    public Vector3 openScale = Vector3.one;

    public static bool hasReadReport = false;

    private bool playerNearby = false;
    private bool journalOpen = false;

    private RectTransform journalRect;
    private CanvasGroup journalCanvasGroup;
    private Coroutine journalCoroutine;

    void Start()
    {
        hasReadReport = false;

        if (reportPromptText != null)
        {
            reportPromptText.SetActive(false);
        }

        if (journalPanel != null)
        {
            journalRect = journalPanel.GetComponent<RectTransform>();

            journalCanvasGroup = journalPanel.GetComponent<CanvasGroup>();
            if (journalCanvasGroup == null)
            {
                journalCanvasGroup = journalPanel.AddComponent<CanvasGroup>();
            }

            journalPanel.SetActive(false);

            if (journalRect != null)
            {
                journalRect.anchoredPosition = closedPosition;
                journalRect.localScale = closedScale;
            }

            journalCanvasGroup.alpha = 0f;
        }
    }

    void Update()
    {
        if (playerNearby && !journalOpen && Input.GetKeyDown(interactKey))
        {
            OpenJournal();
        }
    }

    void OpenJournal()
    {
        journalOpen = true;
        hasReadReport = true;

        if (reportPromptText != null)
        {
            reportPromptText.SetActive(false);
        }

        if (journalPanel != null)
        {
            journalPanel.SetActive(true);

            if (journalCoroutine != null)
            {
                StopCoroutine(journalCoroutine);
            }

            journalCoroutine = StartCoroutine(AnimateJournal(true));
        }

        Debug.Log("The Last Report has been read.");
    }

    void CloseJournal()
    {
        if (!journalOpen && journalPanel != null && !journalPanel.activeSelf)
        {
            return;
        }

        journalOpen = false;

        if (journalCoroutine != null)
        {
            StopCoroutine(journalCoroutine);
        }

        journalCoroutine = StartCoroutine(AnimateJournal(false));
    }

    IEnumerator AnimateJournal(bool opening)
    {
        if (journalPanel == null || journalRect == null || journalCanvasGroup == null)
        {
            yield break;
        }

        if (opening)
        {
            journalPanel.SetActive(true);
        }

        Vector2 startPosition = journalRect.anchoredPosition;
        Vector2 targetPosition = opening ? openPosition : closedPosition;

        Vector3 startScale = journalRect.localScale;
        Vector3 targetScale = opening ? openScale : closedScale;

        float startAlpha = journalCanvasGroup.alpha;
        float targetAlpha = opening ? 1f : 0f;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / animationDuration;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            journalRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, smoothProgress);
            journalRect.localScale = Vector3.Lerp(startScale, targetScale, smoothProgress);
            journalCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, smoothProgress);

            yield return null;
        }

        journalRect.anchoredPosition = targetPosition;
        journalRect.localScale = targetScale;
        journalCanvasGroup.alpha = targetAlpha;

        if (!opening)
        {
            journalPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (!journalOpen && reportPromptText != null)
            {
                reportPromptText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (reportPromptText != null)
            {
                reportPromptText.SetActive(false);
            }

            CloseJournal();
        }
    }
}