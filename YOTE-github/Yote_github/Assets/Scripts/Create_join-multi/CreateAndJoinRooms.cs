using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{


    public TMP_InputField CreateRoomtext;
    public TMP_InputField JoinRoomtext;


    public Room_Item roomItemPrefab;
    List<Room_Item> roomItemsList = new List<Room_Item>();
    public Transform contentObject;

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

   // public override void OnRoomListUpdate(List<RoomInfo> roomList)
   // {
  //      UpdateRoomList(roomList);
  //  }
 //   void UpdateRoomList(List<RoomInfo> list)
  //  {
   //     foreach (Room_Item roomItem in roomItemsList)
   //     {
   //         Destroy(roomItem.gameObject);
   //     }
   //     roomItemsList.Clear();


     //   foreach(RoomInfo room in list)
     //   {
     //      Room_Item newRoom= Instantiate(roomItemPrefab, contentObject);
     //       newRoom.SetRoomName(room.Name);
     //       roomItemsList.Add(newRoom);
    //    }
   // }

}
