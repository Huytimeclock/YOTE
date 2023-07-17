using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class ImportSong : MonoBehaviour
{
    [SerializeField] AudioClip BPMSound;
    private string songPath;
    private float BPM = 1.0f;
    public float delaytimebeforestart = 0f;

    // Start is called before the first frame update
    void Start()
    {




       // GameObject readTimeStamp = GameObject.Find("TrackTime");



        // Get the ReadFile script from the game object
      //  TrackTimeSong ReadTime = readTimeStamp.GetComponent<TrackTimeSong>();




        songPath = ReadFile.songPath;

        // Get the BPM value from the text file
        string bpmString = LoadBeatmapList.BPMValue.Trim();
        Debug.Log("BPM string value: " + bpmString);

        if (float.TryParse(bpmString, out float result))
        {
            BPM = result;
            Debug.Log("BPM value: " + BPM);
        }
        else
        {
            UnityEngine.Debug.LogError("BPM value could not be parsed");
        }

        StartCoroutine(StartPlaying(delaytimebeforestart));
    }

    IEnumerator StartPlaying(float delay)
    {
        yield return new WaitForSeconds(delay);

        float bpmSoundDelay = delaytimebeforestart;

        float songDelay = bpmSoundDelay + (60f / BPM) * 3; // 3 is the index of the song clip

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = null;
        UnityEngine.Debug.Log("Songpath la: " + songPath);
        StartCoroutine(LoadAudioClip(songPath, clip =>
        {
            audioSource.clip = clip;
            audioSource.PlayDelayed(songDelay);
        }));

        StartCoroutine(PlayBPMSound(4));
    }











    private IEnumerator PlayBPMSound(int count)
    {
        GameObject readTimeStamp = GameObject.Find("TrackTime");



        // Get the ReadFile script from the game object
        TrackTimeSong ReadTime = readTimeStamp.GetComponent<TrackTimeSong>();





        AudioSource audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < count; i++)
        {
            audioSource.clip = BPMSound;
            audioSource.PlayDelayed(delaytimebeforestart + (60f / BPM) * i);
            audioSource.PlayOneShot(BPMSound, 3f);
            string readTimeeeee = ReadTime.ReturnTimeStamp();
            Debug.Log("readtime la " + readTimeeeee);
            yield return new WaitForSeconds((60f / BPM));
        }
        audioSource.clip = null;
        StartCoroutine(LoadAudioClip(songPath, clip =>
        {
            audioSource.clip = clip;
            audioSource.Play();
            string readTimeeeee = ReadTime.ReturnTimeStamp();
            Debug.Log("readtime la " + readTimeeeee);

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
}


// note: fix the delay of import song to 2 sec