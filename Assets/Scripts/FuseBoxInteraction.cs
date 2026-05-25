using UnityEngine;

public class FuseBoxInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;

    [Header("Factory Lights")]
    public Light[] factoryLights;

    private bool playerNearby = false;
    private bool powerOn = false;

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        foreach (Light light in factoryLights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerNearby && !powerOn && Input.GetKeyDown(KeyCode.E))
        {
            TurnOnPower();
        }
    }

    void TurnOnPower()
    {
        powerOn = true;

        foreach (Light light in factoryLights)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }

        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        Debug.Log("Power restored. Factory lights are now ON.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !powerOn)
        {
            playerNearby = true;

            if (interactText != null)
            {
                interactText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }
}