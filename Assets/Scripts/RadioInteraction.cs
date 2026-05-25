using UnityEngine;

public class RadioInteraction : MonoBehaviour
{
    public GameObject radioPromptText;
    public AudioSource radioAudio;

    private bool playerIsNear = false;
    private bool radioIsPlaying = false;

    void Start()
    {
        if (radioPromptText != null)
        {
            radioPromptText.SetActive(false);
        }
    }

    void Update()
    {
        // Check if audio already finished
        if (radioIsPlaying && radioAudio != null && !radioAudio.isPlaying)
        {
            radioIsPlaying = false;

            // If player is still near the radio, show the prompt again
            if (playerIsNear && radioPromptText != null)
            {
                radioPromptText.SetActive(true);
            }
        }

        // Player presses E near radio
        if (playerIsNear && !radioIsPlaying && Input.GetKeyDown(KeyCode.E))
        {
            PlayRadio();
        }
    }

    void PlayRadio()
    {
        if (radioAudio != null)
        {
            radioAudio.Play();
            radioIsPlaying = true;
        }

        if (radioPromptText != null)
        {
            radioPromptText.SetActive(false);
        }

        Debug.Log("Radio recording played.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;

            if (!radioIsPlaying && radioPromptText != null)
            {
                radioPromptText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;

            if (radioPromptText != null)
            {
                radioPromptText.SetActive(false);
            }
        }
    }
}