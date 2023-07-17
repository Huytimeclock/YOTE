using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Playeritem : MonoBehaviour
{
    public TMP_Text playerName;

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
    }
}