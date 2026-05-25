using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KeypadSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject keypadPromptText;
    public TMP_Text displayText;
    public GameObject winPanel;

    [Header("Settings")]
    public string correctCode = "4379";
    public KeyCode interactKey = KeyCode.E;
    public float winDelay = 0.8f;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private string currentInput = "";
    private bool playerNearby = false;
    private bool usingKeypad = false;
    private bool gameWon = false;
    private bool canContinueAfterWin = false;

    void Start()
    {
        Time.timeScale = 1f;

        currentInput = "";

        if (keypadPromptText != null)
        {
            keypadPromptText.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateDisplay();
    }

    void Update()
    {
        // This must be ABOVE the "if (gameWon) return;" part.
        // This lets the win screen detect any key press.
        if (gameWon && canContinueAfterWin && Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
            return;
        }

        if (gameWon)
        {
            return;
        }

        if (playerNearby && !usingKeypad && Input.GetKeyDown(interactKey))
        {
            StartUsingKeypad();
        }

        if (usingKeypad)
        {
            HandleKeyboardInput();
        }
    }

    void StartUsingKeypad()
    {
        usingKeypad = true;
        currentInput = "";
        UpdateDisplay();

        if (keypadPromptText != null)
        {
            keypadPromptText.SetActive(false);
        }

        Debug.Log("Using keypad. Type the code and press Enter.");
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) AddNumber("0");
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) AddNumber("1");
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) AddNumber("2");
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) AddNumber("3");
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) AddNumber("4");
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) AddNumber("5");
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) AddNumber("6");
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) AddNumber("7");
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) AddNumber("8");
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) AddNumber("9");

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveLastNumber();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckCode();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopUsingKeypad();
        }
    }

    void AddNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            UpdateDisplay();
        }
    }

    void RemoveLastNumber()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    void CheckCode()
    {
        if (currentInput == correctCode)
        {
            StartCoroutine(WinSequence());
        }
        else
        {
            currentInput = "";

            if (displayText != null)
            {
                displayText.text = "WRONG";
            }

            Invoke(nameof(UpdateDisplay), 1f);
        }
    }

    IEnumerator WinSequence()
    {
        gameWon = true;
        usingKeypad = false;

        if (displayText != null)
        {
            displayText.text = "OPEN";
        }

        if (keypadPromptText != null)
        {
            keypadPromptText.SetActive(false);
        }

        yield return new WaitForSeconds(winDelay);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        canContinueAfterWin = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Correct code. Player escaped. Press any key to continue.");
    }

    void StopUsingKeypad()
    {
        usingKeypad = false;
        currentInput = "";
        UpdateDisplay();

        if (playerNearby && keypadPromptText != null && !gameWon)
        {
            keypadPromptText.SetActive(true);
        }

        Debug.Log("Stopped using keypad.");
    }

    void UpdateDisplay()
    {
        if (displayText != null)
        {
            displayText.text = currentInput == "" ? "----" : currentInput;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameWon)
        {
            playerNearby = true;

            if (keypadPromptText != null && !usingKeypad)
            {
                keypadPromptText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            usingKeypad = false;

            if (keypadPromptText != null)
            {
                keypadPromptText.SetActive(false);
            }

            currentInput = "";
            UpdateDisplay();
        }
    }
}