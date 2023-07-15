using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class increasenumber : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI numberText;

    private int number = 0;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon network.");
            return;
        }

        PhotonNetwork.NetworkingClient.EventReceived += OnCustomEvent;
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnCustomEvent;
    }

    public void OnButtonClick()
    {
        number++;
        numberText.text = number.ToString();

        byte eventCode = 1; // Custom event code
        object[] eventData = new object[] { number };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

        PhotonNetwork.RaiseEvent(eventCode, eventData, options, SendOptions.SendReliable);
    }

    private void OnCustomEvent(EventData eventData)
    {
        if (eventData.Code == 1) // Check if it's the custom event with code 1
        {
            object[] data = (object[])eventData.CustomData;
            int receivedNumber = (int)data[0];

            number = receivedNumber;
            numberText.text = number.ToString();
        }
    }
}
