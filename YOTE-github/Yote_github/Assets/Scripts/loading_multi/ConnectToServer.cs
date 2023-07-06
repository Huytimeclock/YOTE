using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = LoadMultiplayer.username; 
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Room_input");
    }

    


}
