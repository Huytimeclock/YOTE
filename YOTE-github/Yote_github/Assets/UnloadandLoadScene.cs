using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadandLoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.UnloadSceneAsync("Gameplay");
        StartCoroutine(LoadMainSceneWithDelay(1.5f));
    }

    private IEnumerator LoadMainSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainLevelScene");
    }
}
