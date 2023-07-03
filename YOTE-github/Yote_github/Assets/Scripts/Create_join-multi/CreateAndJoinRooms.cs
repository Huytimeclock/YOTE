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


        public static string roomName;


    // Start is called before the first frame update
    void Start()
    {
      

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




}
