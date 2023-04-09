using UnityEngine;
using System.Collections;

public class TrackTimeSong : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LogMessage());
    }

    IEnumerator LogMessage()
    {
        double sec=0;
        while (true)
        {
            Debug.Log(sec);
            yield return new WaitForSeconds(0.5f);
            sec += 0.5;
        }
    }
}
