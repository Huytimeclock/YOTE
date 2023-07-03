using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMultiplayer : MonoBehaviour
{
    public GameObject checkAuth;
    private LoginScript loginScript;

    bool isLoginstatus=false;
    private string UID;
    private string username;

    public void loadMultiRoom()
    {
        // Get the Loginscript component from the checkAuth GameObject
        loginScript = checkAuth.GetComponent<LoginScript>();

        if (loginScript != null)
        {
            isLoginstatus = loginScript.getisLogin();
            // Use the isLoginstatus as needed
            UID= loginScript.getUID();
            username= loginScript.getUsername();
        }
        else
        {
            Debug.LogWarning("Loginscript component not found on checkAuth GameObject.");
        }

        if (isLoginstatus==false)
        {
            Debug.Log("u suck");
        }
        if(isLoginstatus ==true)
        {
            SceneManager.LoadScene("Loading_to_waitingroom", LoadSceneMode.Additive);
        }
        
    }

    public string getUID()
    {
        return UID;
    }

    public string getUsername()
    {
        return username;
    }
}