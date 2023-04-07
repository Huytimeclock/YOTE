using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImportSong : MonoBehaviour
{
    AudioSource m_AudioSource;

   


    // Start is called before the first frame update
    void Start()
    {
        string m_path = Application.dataPath;
        Debug.Log(m_path);
        m_path += "\\Game_data\\Beatmaps\\Test-beatmap\\audio.mp3";
        Debug.Log(m_path);
        StartCoroutine(LoadAudioClip(m_path, clip =>
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
