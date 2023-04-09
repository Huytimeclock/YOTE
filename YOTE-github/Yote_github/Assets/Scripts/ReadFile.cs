using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using System.Collections;
using Unity.VisualScripting;



public class ReadFile : MonoBehaviour
{
    private float startTime; // time when the script starts
    private bool isStarted; // whether the script has started reading the file

    [SerializeField] float enlargeTime;
    protected float enlargeRate ; // Rate of enlargement per second

    //Unity.VisualScripting.Timer t = new Unity.VisualScripting.Timer();


    //Row 1
    [SerializeField] GameObject CreateObjectQ;
    [SerializeField] GameObject CreateObjectW;
    [SerializeField] GameObject CreateObjectE;
    [SerializeField] GameObject CreateObjectR;
    [SerializeField] GameObject CreateObjectT;
    [SerializeField] GameObject CreateObjectY;
    [SerializeField] GameObject CreateObjectU;
    [SerializeField] GameObject CreateObjectI;
    [SerializeField] GameObject CreateObjectO;
    [SerializeField] GameObject CreateObjectP;


    //Row 2
    [SerializeField] GameObject CreateObjectA;
    [SerializeField] GameObject CreateObjectS;
    [SerializeField] GameObject CreateObjectD;
    [SerializeField] GameObject CreateObjectF;
    [SerializeField] GameObject CreateObjectG;
    [SerializeField] GameObject CreateObjectH;    
    [SerializeField] GameObject CreateObjectJ;
    [SerializeField] GameObject CreateObjectK;
    [SerializeField] GameObject CreateObjectL;


    //Row 3
    [SerializeField] GameObject CreateObjectZ;
    [SerializeField] GameObject CreateObjectX;
    [SerializeField] GameObject CreateObjectC;
    [SerializeField] GameObject CreateObjectV;
    [SerializeField] GameObject CreateObjectB;
    [SerializeField] GameObject CreateObjectN;
    [SerializeField] GameObject CreateObjectM;




    void Start()
    {


        enlargeRate = 100 / enlargeTime;


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
                                UnityEngine.Debug.Log("log time la: " + logTime);
                                StartCoroutine(EnlargeObject(logTime-1, getkey));
                                callDebug(logTime, getkey);
                                
                                
                                getkey = "";//reset
                                continue;
                            }
                            getkey += key[i];   //lay cac bien
                            UnityEngine.Debug.Log(getkey);
                            if (i == key.Length - 1)
                            {
                                UnityEngine.Debug.Log("log time la: " + logTime);
                                StartCoroutine(EnlargeObject(logTime - 1, getkey));
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



    IEnumerator EnlargeObject(float triggerTime, string key)
    {
        GameObject EnlargeObject = new GameObject();
       
        switch(key)
        {
            case "Q":
                EnlargeObject = CreateObjectQ;
                break;

            case "W":
                EnlargeObject = CreateObjectW;
                break;

            case "E":
                EnlargeObject = CreateObjectE;
                break;

            case "R":
                EnlargeObject = CreateObjectR;
                break;

            case "T":
                EnlargeObject = CreateObjectT;
                break;

            case "Y":
                EnlargeObject = CreateObjectY;
                break;

            case "U":
                EnlargeObject = CreateObjectU;
                break;

            case "I":
                EnlargeObject = CreateObjectI;
                break;

            case "O":
                EnlargeObject = CreateObjectO;
                break;

            case "P":
                EnlargeObject = CreateObjectP;
                break;

            case "A":
                EnlargeObject = CreateObjectA;
                break;

            case "S":
                EnlargeObject = CreateObjectS;
                break;

            case "D":
                EnlargeObject = CreateObjectD;
                break;

            case "F":
                EnlargeObject = CreateObjectF;
                break;

            case "G":
                EnlargeObject = CreateObjectG;
                break;

            case "H":
                EnlargeObject = CreateObjectH;
                break;

            case "J":
                EnlargeObject = CreateObjectJ;
                break;

            case "K":
                EnlargeObject = CreateObjectK;
                break;

            case "L":
                EnlargeObject = CreateObjectL;
                break;

            case "Z":
                EnlargeObject = CreateObjectZ;
                break;

            case "X":
                EnlargeObject = CreateObjectX;
                break;

            case "C":
                EnlargeObject = CreateObjectC;
                break;

            case "V":
                EnlargeObject = CreateObjectV;
                break;

            case "B":
                EnlargeObject = CreateObjectB;
                break;

            case "N":
                EnlargeObject = CreateObjectN;
                break;

            case "M":
                EnlargeObject = CreateObjectM;
                break;
        }

        while (Time.time < triggerTime)
        {
            yield return null;
        }
        float startTime = triggerTime;
        UnityEngine.Debug.Log("start time la: " + startTime);
        
        float endTime = triggerTime + enlargeTime;
        UnityEngine.Debug.Log("enlarge time la: " + enlargeTime);
        UnityEngine.Debug.Log("end time la: " + endTime);
        while (Time.time < endTime)
        {
            
            float timeElapsed = Time.time - startTime;
            float scale = timeElapsed * enlargeRate;
            EnlargeObject.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        UnityEngine.Debug.Log(Time.time + " Da hoan thanh xong ");
        EnlargeObject.transform.localScale = new Vector3(0f, 0f, 0f);
    }






}

