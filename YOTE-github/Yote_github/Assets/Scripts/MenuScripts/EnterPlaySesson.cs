using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPlaySesson : MonoBehaviour
{
    

    private void Update()
    {
        // bool CheckOnEnter = FindObjectOfType<EnterLogo>().ReturnOnEnter();  ---------------- will use later
    }

    private void OnMouseDown()
    {
        ReadFile.isMulti = false;
        SceneManager.LoadScene("MainLevelScene");
    }

}
