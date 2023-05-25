using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public GameObject pauseOverlay;
    public GameObject pauseButton;


    private bool isPaused = false;

    private void Start()
    {

        pauseOverlay.SetActive(false);
        pauseButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                //ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
    public void PauseGame()
    {
        pauseOverlay.SetActive(true);
        pauseButton.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0.0f;
        isPaused=true;

    }
    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        pauseButton.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    public void Retry()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
