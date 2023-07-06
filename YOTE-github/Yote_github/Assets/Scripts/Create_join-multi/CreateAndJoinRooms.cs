using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{


    public TMP_InputField CreateRoomtext;
    public TMP_InputField JoinRoomtext;

    
    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
 

    public static string roomName;


    // Start is called before the first frame update
    void Start()
    {

        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {   
        if(CreateRoomtext.text.Length>=1)
        {
            roomName = CreateRoomtext.text;
            PhotonNetwork.CreateRoom(CreateRoomtext.text, new RoomOptions() { MaxPlayers=3});

        }
        
    }

    public void JoinRoom()
    {
        if (JoinRoomtext.text.Length >= 1)
        {
            roomName = JoinRoomtext.text;
            PhotonNetwork.JoinRoom(JoinRoomtext.text);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Waiting room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);

    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach ( RoomInfo room in list )
        {
           RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom); 
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void DisconnectAndGoBack()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("SampleScene");
    }
}
