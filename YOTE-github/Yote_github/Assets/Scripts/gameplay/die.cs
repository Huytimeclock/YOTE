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
 
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {

        SceneManager.LoadScene("MainLevelScene");



        // Unload the current scene "Gameplay"
        //SceneManager.UnloadSceneAsync("Gameplay");
    }
    public void Retry()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
