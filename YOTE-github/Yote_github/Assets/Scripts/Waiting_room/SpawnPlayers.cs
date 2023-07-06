using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject scrollViewContent;
    public TMP_Text playerNameText;
    public TMP_Text RoomText;



    // Start is called before the first frame update
    void Start()
    {
        GameObject newObj = PhotonNetwork.Instantiate(objectPrefab.name, Vector3.zero, Quaternion.identity);
        newObj.transform.SetParent(scrollViewContent.transform, false);

        // Get the TMP_Text component in the spawned prefab
        TMP_Text spawnedPlayerNameText = newObj.GetComponentInChildren<TMP_Text>();
        RoomText.text = RoomItem.RoomNameForJoin;
        

        if(RoomItem.RoomNameForJoin.Length==0)
        {
            RoomText.text = CreateAndJoinRooms.roomName;
        }

        if (spawnedPlayerNameText != null)
        {
            // Access and change the name of the TMP_Text component using the player name variable
            spawnedPlayerNameText.text = LoadMultiplayer.username;
        }
        else
        {
            Debug.LogWarning("TMP_Text component not found in the spawned prefab.");
        }

        // Customize the properties of the new object if needed
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Room_input");
    }

}