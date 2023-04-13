using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;


public class ReadFile : MonoBehaviour
{
    private float startTime; // time when the script starts
    private bool isStarted; // whether the script has started reading the file
                            //Unity.VisualScripting.Timer t = new Unity.VisualScripting.Timer();



    #region EnlargeVariable
    // those variable will affect the enlarge of custom box creating
    [SerializeField] float enlargeTime;
    protected float enlargeRate ; // Rate of enlargement per second
    #endregion
    #region Variable
    //variable that will define each object for each box
    //Row 1 default
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


    //Row 2 default
    [SerializeField] GameObject CreateObjectA;
    [SerializeField] GameObject CreateObjectS;
    [SerializeField] GameObject CreateObjectD;
    [SerializeField] GameObject CreateObjectF;
    [SerializeField] GameObject CreateObjectG;
    [SerializeField] GameObject CreateObjectH;    
    [SerializeField] GameObject CreateObjectJ;
    [SerializeField] GameObject CreateObjectK;
    [SerializeField] GameObject CreateObjectL;


    //Row 3 default
    [SerializeField] GameObject CreateObjectZ;
    [SerializeField] GameObject CreateObjectX;
    [SerializeField] GameObject CreateObjectC;
    [SerializeField] GameObject CreateObjectV;
    [SerializeField] GameObject CreateObjectB;
    [SerializeField] GameObject CreateObjectN;
    [SerializeField] GameObject CreateObjectM;


    //Row 1 air
    [SerializeField] GameObject CreateObjectAirQ;
    [SerializeField] GameObject CreateObjectAirW;
    [SerializeField] GameObject CreateObjectAirE;
    [SerializeField] GameObject CreateObjectAirR;
    [SerializeField] GameObject CreateObjectAirT;
    [SerializeField] GameObject CreateObjectAirY;
    [SerializeField] GameObject CreateObjectAirU;
    [SerializeField] GameObject CreateObjectAirI;
    [SerializeField] GameObject CreateObjectAirO;
    [SerializeField] GameObject CreateObjectAirP;


    //Row 2 air
    [SerializeField] GameObject CreateObjectAirA;
    [SerializeField] GameObject CreateObjectAirS;
    [SerializeField] GameObject CreateObjectAirD;
    [SerializeField] GameObject CreateObjectAirF;
    [SerializeField] GameObject CreateObjectAirG;
    [SerializeField] GameObject CreateObjectAirH;
    [SerializeField] GameObject CreateObjectAirJ;
    [SerializeField] GameObject CreateObjectAirK;
    [SerializeField] GameObject CreateObjectAirL;


    //Row 3 air
    [SerializeField] GameObject CreateObjectAirZ;
    [SerializeField] GameObject CreateObjectAirX;
    [SerializeField] GameObject CreateObjectAirC;
    [SerializeField] GameObject CreateObjectAirV;
    [SerializeField] GameObject CreateObjectAirB;
    [SerializeField] GameObject CreateObjectAirN;
    [SerializeField] GameObject CreateObjectAirM;
    //--------------------------------------------------------------------------------------------------
    #endregion

    private struct Note
    {
        public float time;
        public string key;
        public bool NeedShift;
        public bool NoteStatus;
        //16 -> 32 -> 64

    }

    List<Note> notes = new List<Note>();


    void Start()
    {
        // related to EnlargeObject
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
              
                UnityEngine.Debug.Log(line);
                string[] splitLine = line.Split('['); // split the line at each '[' character


                if (splitLine.Length >= 2) // make sure there are at least 2 parts (time and key)
                {

                    // Debug.Log(splitLine[1].TrimEnd
                    // (']') + "test");
                    // Debug.Log(splitLine[2].TrimEnd(']') + "test");


                    float time; // identified time through text file


                    if (float.TryParse(splitLine[1].TrimEnd(']'), out time)) // try to parse the time from the second part
                    {

                        string key = splitLine[2].TrimEnd(']'); // get the key from the third part

                        string getkey = ""; // get key from the key ( exp : b,c is the key so b and c will be in get key )

                        float logTime = startTime + time; // calculate the time when the log should be written

                        // phan tich cac bien va thoi gian
                        for (int i = 0; i < key.Length; i++) // used to help when there is so many input ( exp [h,i,j] )
                        {
                            if (key[i] == ',')
                            {
                                //UnityEngine.Debug.Log("log time la: " + logTime);  // debug log thoi gian can de in ra dong lenh phia sau
                                StartCoroutine(EnlargeObject(logTime - enlargeTime, getkey)); // bat dau enlarge object vs 1 chut offset ( logtime - enlargetime )
                                if (getkey.Length == 1)
                                {
                                    notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false,  });
                                }
                                

                                callDebug(logTime, getkey);
                                
                                
                                getkey = "";//reset
                                continue;
                            }


                            getkey += key[i];   //lay cac bien
                            UnityEngine.Debug.Log(getkey);


                            if (i == key.Length - 1)
                            {
                                //UnityEngine.Debug.Log("log time la: " + logTime);
                                StartCoroutine(EnlargeObject(logTime - enlargeTime, getkey));
                                notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false, });

                                callDebug(logTime, getkey);
                                
                            }


                        }

                        // ghi ra cac bien va thoi gian
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

            case "QQ":
                EnlargeObject = CreateObjectAirQ;
                break;

            case "WW":
                EnlargeObject = CreateObjectAirW;
                break;

            case "EE":
                EnlargeObject = CreateObjectAirE;
                break;

            case "RR":
                EnlargeObject = CreateObjectAirR;
                break;

            case "TT":
                EnlargeObject = CreateObjectAirT;
                break;

            case "YY":
                EnlargeObject = CreateObjectAirY;
                break;

            case "UU":
                EnlargeObject = CreateObjectAirU;
                break;

            case "II":
                EnlargeObject = CreateObjectAirI;
                break;

            case "OO":
                EnlargeObject = CreateObjectAirO;
                break;

            case "PP":
                EnlargeObject = CreateObjectAirP;
                break;

            case "AA":
                EnlargeObject = CreateObjectAirA;
                break;

            case "SS":
                EnlargeObject = CreateObjectAirS;
                break;

            case "DD":
                EnlargeObject = CreateObjectAirD;
                break;

            case "FF":
                EnlargeObject = CreateObjectAirF;
                break;

            case "GG":
                EnlargeObject = CreateObjectAirG;
                break;

            case "HH":
                EnlargeObject = CreateObjectAirH;
                break;

            case "JJ":
                EnlargeObject = CreateObjectAirJ;
                break;

            case "KK":
                EnlargeObject = CreateObjectAirK;
                break;

            case "LL":
                EnlargeObject = CreateObjectAirL;
                break;

            case "ZZ":
                EnlargeObject = CreateObjectAirZ;
                break;

            case "XX":
                EnlargeObject = CreateObjectAirX;
                break;

            case "CC":
                EnlargeObject = CreateObjectAirC;
                break;

            case "VV":
                EnlargeObject = CreateObjectAirV;
                break;

            case "BB":
                EnlargeObject = CreateObjectAirB;
                break;

            case "NN":
                EnlargeObject = CreateObjectAirN;
                break;

            case "MM":
                EnlargeObject = CreateObjectAirM;
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

    void ListNote(List<Note> notes)
    {
        for (int i =0;i< notes.Count; i++)
        {
            Note note = notes[i];
        }
    }

    IEnumerator CreateNote (float Atime, string key)
    {
        while (Time.time < Atime) //active time
        {
            yield return null;
        }
        float startTime = Atime - 0.064f;
        float endtime = Atime + 0.064f;
        while (Time.time < endtime)
        {
            float timeelapsed = Time.time - startTime;
           if (Input.GetKey (key))
            {
                if (timeelapsed <=0.016 && timeelapsed >=0)
                {
                    UnityEngine.Debug.Log("Good");
                }
                if (timeelapsed<=0.032 && timeelapsed >0.016)
                {
                    UnityEngine.Debug.Log("Great");
                }
                if (timeelapsed <= 0.048 && timeelapsed > 0.032)
                {
                    UnityEngine.Debug.Log("Perfect");
                }
            }
        }


       
    }
    // tuong tu enlarge object but se tao ra cac time delay de bam



    // y tuong: cu goi 1 cai ham cho tung log _> trust...
    // create many object at the sameplace
}

