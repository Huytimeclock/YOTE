using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using System.Collections;

public class ReadFile : MonoBehaviour
{
    private float startTime; // time when the script starts
    private bool isStarted; // whether the script has started reading the file

    public float enlargeTime = 3.0f; // Time to enlarge the object
    public float enlargeRate = 33f; // Rate of enlargement per second


    [SerializeField] GameObject CreateObjectH;
    
    [SerializeField] GameObject CreateObjectJ;
    

    void Start()
    {

        



        UnityEngine.Debug.ClearDeveloperConsole();
        startTime = Time.time;
        isStarted = false;


    }
    private void Update()
    {

        if (!isStarted)
        {
            string filePath = Application.dataPath + "\\Game_data\\Beatmaps\\Test-beatmap\\map.txt";
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                UnityEngine.Debug.LogError("File not found at: " + filePath);
                return;
            }

            // Read the file and log each line
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Debug.Log(line);
                string[] splitLine = line.Split('['); // split the line at each '[' character
                if (splitLine.Length >= 2) // make sure there are at least 2 parts (time and key)
                {
                    // Debug.Log(splitLine[1].TrimEnd(']') + "test");
                    // Debug.Log(splitLine[2].TrimEnd(']') + "test");
                    float time;
                    if (float.TryParse(splitLine[1].TrimEnd(']'), out time)) // try to parse the time from the second part
                    {

                        string key = splitLine[2].TrimEnd(']'); // get the key from the third part
                        string getkey = ""; // get key from different element in key
                        float logTime = startTime + time; // calculate the time when the log should be written


                        for (int i = 0; i < key.Length; i++) // used to help when there is so many input ( exp [h,i,j] )
                        {
                            if (key[i] == ',')
                            {

                                StartCoroutine(EnlargeObject());
                                callDebug(logTime, getkey);
                                
                                getkey = "";//reset
                                continue;
                            }
                            getkey += key[i];   //lay cac bien
                            UnityEngine.Debug.Log(getkey);
                            if (i == key.Length - 1)
                            {
                                StartCoroutine(EnlargeObject());
                                callDebug(logTime, getkey);
                                
                            }


                        }


                        void callDebug(float xdtime, string xdkey)
                        {
                            StartCoroutine(LogAtTime(logTime, xdkey)); // start the coroutine to write the log at the specified time
                        }

                    }
                }
            }

        }
        isStarted = true;

    }
    private System.Collections.IEnumerator LogAtTime(float time, string message)
    {
        while (Time.time < time)
        {
            yield return null;
        }

        
        UnityEngine.Debug.Log(message + " " + time);
    }



    IEnumerator EnlargeObject()
    {
        float startTime = Time.time;
        while (Time.time - startTime < enlargeTime)
        {
            float scale = (Time.time - startTime) * enlargeRate;
            CreateObjectH.transform.localScale = new Vector3(scale, scale, scale);
            yield return null; // Wait for the next frame
        }
        // Make sure the object is fully enlarged at the end of the loop
        CreateObjectH.transform.localScale = new Vector3(enlargeRate * enlargeTime, enlargeRate * enlargeTime, enlargeRate * enlargeTime);
    }






}

