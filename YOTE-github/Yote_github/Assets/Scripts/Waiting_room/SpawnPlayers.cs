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

    public List<Playeritem> playerItemsList = new List<Playeritem>();
    public Playeritem playerItemPrefab;
    public Transform playerItemParent;

    private float timer = 2f; // Timer interval in seconds
    private float elapsedTime = 0f; // Elapsed time since last function call

    private void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the timer interval has elapsed
        if (elapsedTime >= timer)
        {
            // Call the function
            UpdatePlayerList();

            // Reset the elapsed time
            elapsedTime = 0f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerList();

        
        RoomText.text = RoomItem.RoomNameForJoin;
        

        if(RoomItem.RoomNameForJoin.Length==0)
        {
            RoomText.text = CreateAndJoinRooms.roomName;
        }

        
    }

    public void OnClickLeaveRoom()
    {
        
        PhotonNetwork.LeaveRoom();
        UpdatePlayerList();
        SceneManager.LoadScene("Room_input");
    }

    void UpdatePlayerList()
    {
        foreach (Playeritem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        { return; }

        foreach (KeyValuePair<int, Player>player in PhotonNetwork.CurrentRoom.Players)
        {
           Playeritem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(newPlayerItem);
        }
    }

}