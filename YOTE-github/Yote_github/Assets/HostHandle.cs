using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HostHandle : MonoBehaviour
{
    public GameObject ChooseSongButton;
    public GameObject PlayButton;
    // Start is called before the first frame update
    void Start()
    {
        if(CreateAndJoinRooms.isHost==false)
        {
            ChooseSongButton.SetActive(false);
            PlayButton.SetActive(false);
        }
        if(CreateAndJoinRooms.isHost == true)
        {
            ChooseSongButton.SetActive(true);
            PlayButton.SetActive(true);
        }
    }

}
