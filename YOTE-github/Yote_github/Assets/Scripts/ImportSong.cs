using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImportSong : MonoBehaviour
{
    AudioSource m_AudioSource;
    private string songPath;
   


    // Start is called before the first frame update
    void Start()
    {

        // Find the ReadFile game object in Scene 1
        GameObject readFileObj = GameObject.Find("ScriptHandleLoadSong");


        if (readFileObj == null)
        {
            UnityEngine.Debug.LogError("LoadBeatmapList object not found");
            return;
        }

        // Get the ReadFile script from the game object
        LoadBeatmapList readFile = readFileObj.GetComponent<LoadBeatmapList>();

        if (readFile == null)
        {
            UnityEngine.Debug.LogError("LoadBeatmapList script not found");
            return;
        }
        songPath = readFile.GetSongPath1().ToString();



        StartCoroutine(LoadAudioClip(songPath, clip =>
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }));
    }
    private IEnumerator LoadAudioClip(string url, System.Action<AudioClip> onComplete)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading audio clip: " + www.error);
                onComplete?.Invoke(null);
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            onComplete?.Invoke(clip);
        }
    }

   // private double lastLogTime;
   // private const double logInterval = 1.0; // write every log each second

//    private void OnAudioFilterRead(float[] data, int channels)
  //  {
    //    double currentTime = AudioSettings.dspTime;
      //  double elapsedTime = currentTime - lastLogTime;

        //if (elapsedTime >= logInterval)
        //{
          //  Debug.Log("Audio is playing...");
            //lastLogTime = currentTime;
        //}
    //}

}
