using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomText;
    CreateAndJoinRooms manager;

    public static string RoomNameForJoin="";

    private void Start()
    {
       manager = FindObjectOfType<CreateAndJoinRooms>();
    }

    public void SetRoomName(string _roomName)
    {
        roomText.text = _roomName;
    }

    public void OnClickItem()
    {
        RoomNameForJoin = roomText.text;   
        manager.JoinRoom(roomText.text);
    }


}
