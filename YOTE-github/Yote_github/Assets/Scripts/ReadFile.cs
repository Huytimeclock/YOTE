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
using System.Diagnostics.CodeAnalysis;

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


    //Fuck
    [SerializeField] GameObject[] buttonaaa;
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
                                int convertKey;
                                bool isAir = false;
                                //UnityEngine.Debug.Log("log time la: " + logTime);  // debug log thoi gian can de in ra dong lenh phia sau
                                


                                StartCoroutine(EnlargeObject(logTime - enlargeTime, getkey)); // bat dau enlarge object vs 1 chut offset ( logtime - enlargetime )
                                if (getkey.Length == 1)
                                {
                                    int needshiftnum = 0;
                                    if (getkey.Length == 1 && char.IsUpper(getkey[0]))
                                    {
                                        getkey = char.ToLower(getkey[0]).ToString();
                                        needshiftnum = 1;
                                    }
                                    if (needshiftnum==1)
                                    {
                                        notes.Add(new Note { time = logTime, key = getkey, NeedShift = true, NoteStatus = false, });
                                    }
                                    if (needshiftnum == 0)
                                    {
                                        notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false, });
                                    }
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
                                int needshiftnum = 0;
                                if (getkey.Length == 1 && char.IsUpper(getkey[0]))
                                {
                                    getkey = char.ToLower(getkey[0]).ToString();
                                    needshiftnum = 1;
                                }
                                if (needshiftnum == 1)
                                {
                                    notes.Add(new Note { time = logTime, key = getkey, NeedShift = true, NoteStatus = false, });
                                }
                                if (needshiftnum == 0)
                                {
                                    notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false, });
                                }

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
            StartCoroutine(ListNote(notes)); 

        }
        
        isStarted = true;

    }

    int returnButtonType (string key, out int convertkey, out bool isAir)
    {

        switch (key)
        {
            case "q":
                //EnlargeObject = CreateObjectQ;
                convertkey = 0;
                isAir = false;
                break;

            case "w":
                //EnlargeObject = CreateObjectW;
                convertkey = 1;
                isAir = false;
                break;

            case "e":
                //EnlargeObject = CreateObjectE;
                convertkey = 2;
                isAir = false;
                break;

            case "r":
                //EnlargeObject = CreateObjectR;
                numKey = 3;
                break;

            case "t":
                //EnlargeObject = CreateObjectT;
                numKey = 4;
                break;

            case "y":
                //EnlargeObject = CreateObjectY;
                numKey = 5;
                break;

            case "u":
                numKey = 6;
                //EnlargeObject = CreateObjectU;
                break;

            case "i":
                //EnlargeObject = CreateObjectI;
                numKey = 7;
                break;

            case "o":
                // EnlargeObject = CreateObjectO;
                numKey = 8;
                break;

            case "p":
                // EnlargeObject = CreateObjectP;
                break;

            case "a":
                //  EnlargeObject = CreateObjectA;
                break;

            case "s":
                //  EnlargeObject = CreateObjectS;
                break;

            case "d":
                //   EnlargeObject = CreateObjectD;
                break;

            case "f":
                //  EnlargeObject = CreateObjectF;
                break;

            case "g":
                //  EnlargeObject = CreateObjectG;
                break;

            case "h":
                //    EnlargeObject = CreateObjectH;
                break;

            case "j":
                //  EnlargeObject = CreateObjectJ;
                break;

            case "k":
                //  EnlargeObject = CreateObjectK;
                break;

            case "l":
                //  EnlargeObject = CreateObjectL;
                break;

            case "z":
                //    EnlargeObject = CreateObjectZ;
                break;

            case "x":
                //    EnlargeObject = CreateObjectX;
                break;

            case "c":
                //      EnlargeObject = CreateObjectC;
                break;

            case "v":
                //  EnlargeObject = CreateObjectV;
                break;

            case "b":
                //   EnlargeObject = CreateObjectB;
                break;

            case "n":
                //   EnlargeObject = CreateObjectN;
                break;

            case "m":
                //     EnlargeObject = CreateObjectM;
                break;

            case "Q":
                //   EnlargeObject = CreateObjectAirQ;
                break;

            case "W":
                //    EnlargeObject = CreateObjectAirW;
                break;

            case "E":
                //   EnlargeObject = CreateObjectAirE;
                break;

            case "R":
                //    EnlargeObject = CreateObjectAirR;
                break;

            case "T":
                //    EnlargeObject = CreateObjectAirT;
                break;

            case "Y":
                //  EnlargeObject = CreateObjectAirY;
                break;

            case "U":
                //   EnlargeObject = CreateObjectAirU;
                break;

            case "I":
                //  EnlargeObject = CreateObjectAirI;
                break;

            case "O":
                //   EnlargeObject = CreateObjectAirO;
                break;

            case "P":
                //   EnlargeObject = CreateObjectAirP;
                break;

            case "A":
                //   EnlargeObject = CreateObjectAirA;
                break;

            case "S":
                //   EnlargeObject = CreateObjectAirS;
                break;

            case "D":
                //  EnlargeObject = CreateObjectAirD;
                break;

            case "F":
                //   EnlargeObject = CreateObjectAirF;
                break;

            case "G":
                //   EnlargeObject = CreateObjectAirG;
                break;

            case "H":
                //   EnlargeObject = CreateObjectAirH;
                break;

            case "J":
                //    EnlargeObject = CreateObjectAirJ;
                break;

            case "K":
                //   EnlargeObject = CreateObjectAirK;
                break;

            case "L":
                //   EnlargeObject = CreateObjectAirL;
                break;

            case "ZZ":
                //   EnlargeObject = CreateObjectAirZ;
                break;

            case "X":
                //  EnlargeObject = CreateObjectAirX;
                break;

            case "C":
                //  EnlargeObject = CreateObjectAirC;
                break;

            case "V":
                //  EnlargeObject = CreateObjectAirV;
                break;

            case "B":
                //    EnlargeObject = CreateObjectAirB;
                break;

            case "N":
                //    EnlargeObject = CreateObjectAirN;
                break;

            case "M":
                //  EnlargeObject = CreateObjectAirM;
                break;
        }

        return numKey;
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

    IEnumerator ListNote(List<Note> notes)
    {
        for (int i =0;i< notes.Count; i++)
        {
            Note note = notes[i];               
            yield return StartCoroutine(CreateNote(note.time, note.key, note.NeedShift));           
        }
    }

    IEnumerator CreateNote(float Atime, string key, bool needshift)
    {
        //UnityEngine.Debug.Log("key la: " + key);
        while (Time.time < Atime) //active time
        {
            yield return null;
        }


        float startTime = Atime - 0.064f;
        //UnityEngine.Debug.Log("start time create note la: " + startTime);
        float endtime = Atime + 0.064f;
        //UnityEngine.Debug.Log("start time end note la: " + endtime);


        if (needshift == false)
        {
            while (true) // runs continuously until the coroutine is stopped
            {
                if (Time.time >= startTime && Time.time < endtime) // check if Time.time is within the desired range
                {
                    float timeelapsed = Time.time - startTime;
                    if (Input.GetKey(key))
                    {
                        LogResultClick(timeelapsed);
                        break;
                    }
                }
                else if (Time.time >= endtime) // check if Time.time has exceeded the endtime
                {
                    UnityEngine.Debug.Log("Goodbye");
                    yield break; // exit the coroutine
                }
                yield return null;
            }

        }


        if (needshift == true)
        {
            bool isShiftPressed = false;
            while (true) // runs continuously until the coroutine is stopped
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isShiftPressed = true;
                }
                if (Time.time >= startTime && Time.time < endtime) // check if Time.time is within the desired range
                {
                    float timeelapsed = Time.time - startTime;
                    if (Input.GetKey(key)&&isShiftPressed==true)
                    {
                        LogResultClick(timeelapsed);
                        break;
                    }
                }
                else if (Time.time >= endtime) // check if Time.time has exceeded the endtime
                {
                    UnityEngine.Debug.Log("Goodbye");
                    yield break; // exit the coroutine
                }
                yield return null;
            }
        }


    }
    // tuong tu enlarge object but se tao ra cac time delay de bam


    void LogResultClick (float timeelapsed)
    {
        UnityEngine.Debug.Log("Ban da click vao luc: " + timeelapsed);
        if (timeelapsed <= 0.016 && timeelapsed >= 0)
        {
            UnityEngine.Debug.Log("Good");
        }
        else if (timeelapsed <= 0.032)
        {
            UnityEngine.Debug.Log("Great");
        }
        else if (timeelapsed <= 0.048)
        {
            UnityEngine.Debug.Log("Perfect");
        }
        else if (timeelapsed <= 0.08)
        {
            UnityEngine.Debug.Log("CriticalPerfect");
        }
        else if (timeelapsed <= 0.096)
        {
            UnityEngine.Debug.Log("Perfect");
        }
        else if (timeelapsed <= 0.112)
        {
            UnityEngine.Debug.Log("Great");
        }
        else if (timeelapsed <= 0.128)
        {
            UnityEngine.Debug.Log("Good");
        }
    }

    // create many object at the sameplace
}

