using UnityEngine;

public class TrackTimeSong : MonoBehaviour
{
    double startTime;

    void Start()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public string ReturnTimeStamp()
    {
        double elapsed = Time.realtimeSinceStartup - startTime;
        return  elapsed.ToString("0.000");
    }
}
