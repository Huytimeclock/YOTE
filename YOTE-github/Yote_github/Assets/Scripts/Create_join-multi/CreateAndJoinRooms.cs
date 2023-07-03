using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{


    public TMP_InputField CreateRoomtext;
    public TMP_InputField JoinRoomtext;

    private string UID;
    private string username;
    private string roomName;
    // Start is called before the first frame update
    void Start()
    {
        GameObject readFileObj = GameObject.Find("ConnectToServer");

        if (readFileObj == null)
        {
            Debug.LogError("ConnectToServer object not found");
            return;
        }

        ConnectToServer loadConnectToServerScript = readFileObj.GetComponent<ConnectToServer>();

        if (loadConnectToServerScript == null)
        {
            Debug.LogError("loadConnectToServerScript script not found");
            return;
        }

        UID = loadConnectToServerScript.getUID();
        username = loadConnectToServerScript.getUsername();
        SceneManager.UnloadSceneAsync("SampleScene");


        Debug.Log("UID: " + UID);
        Debug.Log("Username: " + username);

    }

    public void CreateRoom()
    {
        roomName = CreateRoomtext.text;
        PhotonNetwork.CreateRoom(CreateRoomtext.text);
    }

    public void JoinRoom()
    {
        roomName = CreateRoomtext.text;
        PhotonNetwork.JoinRoom(JoinRoomtext.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Waiting room");
    }

    public string getUID()
    {
        return UID;
    }

    public string getUsername()
    {
        return username;
    }

    public string getRoomName()
    {
        return roomName;
    }


}
