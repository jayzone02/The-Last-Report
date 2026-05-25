using System.Collections;
using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("UI Prompt")]
    public GameObject keycardPromptText;

    [Header("Item Acquired Popup")]
    public ItemAcquiredPopup acquiredPopup;

    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Animation & Audio")]
    public float pickupAnimationDuration = 0.35f;
    public AudioClip pickupClip;
    public AudioSource audioSource;

    public static bool hasKeycard = false;

    private bool playerNearby = false;
    private bool isPickingUp = false;
    private Transform playerTransform;

    void Start()
    {
        hasKeycard = false;

        if (keycardPromptText != null)
        {
            keycardPromptText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNearby && !isPickingUp && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(PickUpKeycardSmooth());
        }
    }

    void PickUpKeycard()
    {
        hasKeycard = true;

        if (keycardPromptText != null)
        {
            keycardPromptText.SetActive(false);
        }

        if (acquiredPopup != null)
        {
            acquiredPopup.ShowPopup("Keycard Acquired");
        }

        Debug.Log("Keycard picked up!");

        gameObject.SetActive(false);
    }

    private IEnumerator PickUpKeycardSmooth()
    {
        isPickingUp = true;

        if (pickupClip != null)
        {
            PlaySound(pickupClip);
        }

        Collider col = GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (col != null)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = playerTransform != null ? playerTransform.position : startPos;
        Quaternion endRot = playerTransform != null ? playerTransform.rotation : startRot;

        float t = 0f;

        while (t < pickupAnimationDuration)
        {
            t += Time.deltaTime;

            float f = Mathf.SmoothStep(0f, 1f, t / pickupAnimationDuration);

            transform.position = Vector3.Lerp(startPos, endPos, f);
            transform.rotation = Quaternion.Slerp(startRot, endRot, f);

            yield return null;
        }

        PickUpKeycard();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            return;
        }

        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered keycard trigger: " + other.name);

        if (other.CompareTag("Player") && !hasKeycard && !isPickingUp)
        {
            playerNearby = true;
            playerTransform = other.transform;

            if (keycardPromptText != null)
            {
                keycardPromptText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited keycard trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (keycardPromptText != null)
            {
                keycardPromptText.SetActive(false);
            }
        }
    }
}