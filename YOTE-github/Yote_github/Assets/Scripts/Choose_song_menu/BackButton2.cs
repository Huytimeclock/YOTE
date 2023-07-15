using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.UnloadSceneAsync("MainLevelScene");
    }
}
