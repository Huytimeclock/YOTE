using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private string UID;
    private string username;
    // Start is called before the first frame update
    void Start()
    {
        GameObject readFileObj = GameObject.Find("LoadMultiplayer");

        if (readFileObj == null)
        {
            Debug.LogError("LoadMultiplayer object not found");
            return;
        }

        LoadMultiplayer loadMultiplayerScript = readFileObj.GetComponent<LoadMultiplayer>();

        if (loadMultiplayerScript == null)
        {
            Debug.LogError("LoadMultiplayer script not found");
            return;
        }

         UID = loadMultiplayerScript.getUID();
         username = loadMultiplayerScript.getUsername();
        SceneManager.UnloadSceneAsync("SampleScene");


        Debug.Log("UID: " + UID);
        Debug.Log("Username: " + username);

        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Room_input", LoadSceneMode.Additive);
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
