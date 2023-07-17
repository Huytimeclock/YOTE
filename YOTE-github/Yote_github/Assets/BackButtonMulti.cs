using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonMulti : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Waiting room");
    }
}
