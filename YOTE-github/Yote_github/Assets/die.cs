using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class die : MonoBehaviour
{


    private void Start()
    {

    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        Debug.Log ("time.timescale la: " + Time.timeScale);
        AudioListener.pause = false;
        Debug.Log("audiolisterner la: " + AudioListener.pause);
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
