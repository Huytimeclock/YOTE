using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
        if(ReadFile.isMulti==false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && ReadFile.isMulti == false)
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
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainLevelScene");



        // Unload the current scene "Gameplay"
        //SceneManager.UnloadSceneAsync("Gameplay");
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
