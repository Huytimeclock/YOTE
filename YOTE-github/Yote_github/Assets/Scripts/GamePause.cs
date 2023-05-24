using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button continueButton;
    public Button quitButton;

    private bool isPaused = false;

    private void Start()
    {
        continueButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Set the time scale to 0 to freeze time
        pauseMenu.SetActive(true);
        // Add any additional code you want to run when the game is paused
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Set the time scale back to 1 to resume time
        pauseMenu.SetActive(false);
        // Add any additional code you want to run when the game is resumed
    }

    private void QuitGame()
    {
        // Add code here to quit the game, e.g., Application.Quit()
    }
}
