using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{


    public TMP_InputField CreateRoomtext;
    public TMP_InputField JoinRoomtext;
    // Start is called before the first frame update


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomtext.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomtext.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Waiting room");
    }
}
