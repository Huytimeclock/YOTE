using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room_Item : MonoBehaviour
{
    public TMP_Text roomText;

    public void SetRoomName (string _roomName)
    {
        roomText.text = _roomName;
    }
}
