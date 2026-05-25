using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject instructionsPage;

    private PanelZoomAnimation instructionsAnimation;

    void Start()
    {
        if (instructionsPage != null)
        {
            instructionsAnimation = instructionsPage.GetComponent<PanelZoomAnimation>();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("FactoryNight");
    }

    public void ShowInstructions()
    {
        mainMenuPanel.SetActive(false);
        instructionsPage.SetActive(true);
    }

    public void BackToMainMenu()
    {
        if (instructionsAnimation != null)
        {
            instructionsAnimation.PlayClose(() =>
            {
                instructionsPage.SetActive(false);
                mainMenuPanel.SetActive(true);
            });
        }
        else
        {
            instructionsPage.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();
    }
}