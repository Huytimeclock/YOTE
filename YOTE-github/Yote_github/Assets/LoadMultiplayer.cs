using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMultiplayer : MonoBehaviour
{

    public void loadMultiRoom()
    {
        SceneManager.LoadScene("Loading_to_waitingroom");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
