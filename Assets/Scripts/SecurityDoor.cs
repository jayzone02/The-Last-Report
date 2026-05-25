using System.Collections;
using UnityEngine;
using TMPro;

public class SecurityDoor : MonoBehaviour
{
    [Header("UI")]
    public GameObject doorPromptText;
    public TMP_Text doorPromptTMP;

    [Header("Door Settings")]
    public Transform doorToMove;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public KeyCode interactKey = KeyCode.E;

    [Header("Animation")]
    public float textFadeDuration = 0.2f;
    public float doorOpenDuration = 1.0f;

    private bool playerNearby = false;
    private bool doorOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetOpenRotation;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (doorToMove == null)
        {
            doorToMove = transform;
        }

        closedRotation = doorToMove.rotation;
        targetOpenRotation = Quaternion.Euler(doorToMove.eulerAngles + openRotation);

        if (doorPromptText != null)
        {
            doorPromptText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNearby && !doorOpen)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (KeycardPickup.hasKeycard)
                {
                    OpenDoor();
                }
                else
                {
                    ShowMessage("The door is LOCKED");
                }
            }
        }
    }

    void OpenDoor()
    {
        doorOpen = true;

        if (doorPromptText != null)
        {
            doorPromptText.SetActive(false);
        }

        StartCoroutine(OpenDoorSmooth());

        Debug.Log("Security door opened.");
    }

    private IEnumerator OpenDoorSmooth()
    {
        Quaternion startRot = doorToMove.rotation;
        float t = 0f;
        while (t < doorOpenDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.SmoothStep(0f, 1f, t / doorOpenDuration);
            doorToMove.rotation = Quaternion.Slerp(startRot, targetOpenRotation, f);
            yield return null;
        }
        doorToMove.rotation = targetOpenRotation;
    }

    void ShowMessage(string message)
    {
        if (doorPromptTMP != null)
        {
            doorPromptTMP.text = message;
        }

        if (doorPromptText != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeText(true, textFadeDuration));
        }
    }

    private IEnumerator FadeText(bool show, float duration)
    {
        if (doorPromptText == null)
            yield break;

        doorPromptText.SetActive(true);
        Color c = doorPromptTMP.color;
        float start = c.a;
        float end = show ? 1f : 0f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / duration);
            doorPromptTMP.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        doorPromptTMP.color = new Color(c.r, c.g, c.b, end);
        if (!show)
            doorPromptText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doorOpen)
        {
            playerNearby = true;

            if (KeycardPickup.hasKeycard)
            {
                ShowMessage("Press E to open security door");
            }
            else
            {
                ShowMessage("The door is LOCKED");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (doorPromptText != null)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeText(false, textFadeDuration));
            }
        }
    }
}