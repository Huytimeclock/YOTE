using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PlayerListUpdater : MonoBehaviourPunCallbacks
{
    public GameObject playerItemPrefab;
    public Transform playerItemParent;

    private List<PlayerItemMulti> playerItemsList = new List<PlayerItemMulti>();

    private void Awake()
    {

        float myPercentage = ReadFile.percentage; // Get the player's percentage
        object[] eventData3 = new object[] { PhotonNetwork.LocalPlayer.NickName, myPercentage };
        PhotonNetwork.RaiseEvent(30, eventData3, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);

        Debug.Log("PlayerListUpdater Awake");
        PhotonNetwork.NetworkingClient.EventReceived += OnCustomEvent;
    }

    private void OnDestroy()
    {
        Debug.Log("PlayerListUpdater OnDestroy");
        PhotonNetwork.NetworkingClient.EventReceived -= OnCustomEvent;
    }

    private void OnCustomEvent(EventData eventData)
    {
        Debug.Log("Custom event received");
        if (eventData.Code == 30)
        {
            object[] data = (object[])eventData.CustomData;

            // Extract the player's nickname and percentage from the received data
            string nickname = (string)data[0];
            float percentage = (float)data[1];

            // Update the player list with the new player's information
            UpdatePlayerList(nickname, percentage);
        }
    }

    private void UpdatePlayerList(string nickname, float percentage)
    {
        // Check if the player item already exists in the list
        PlayerItemMulti existingPlayerItem = playerItemsList.Find(item => item.playerNameText.text == nickname);

        if (existingPlayerItem != null)
        {
            Debug.Log("Existing player item found: " + nickname);
            // Update the existing player item with the new percentage
            existingPlayerItem.UpdatePercentage(percentage);
        }
        else
        {
            Debug.Log("Creating new player item: " + nickname);
            // Create a new player item for the player
            GameObject playerItemObject = Instantiate(playerItemPrefab, playerItemParent);
            PlayerItemMulti newPlayerItem = playerItemObject.GetComponent<PlayerItemMulti>();
            newPlayerItem.Initialize(nickname, percentage);
            playerItemsList.Add(newPlayerItem);
        }
    }
}
