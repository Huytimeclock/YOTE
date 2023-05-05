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
using TMPro;
using UnityEngine.SceneManagement;




public class ReadFile : MonoBehaviour
{
    private float startTime; // time when the script starts
    private bool isStarted; // whether the script has started reading the file
                            //Unity.VisualScripting.Timer t = new Unity.VisualScripting.Timer();
    float EndNoteTime = 0f;

    public AudioSource audioSource;


    //for score variable
    int numOfNotes = 0;
    float percentage = 0f;
    float goodPercentageValue = 0f;
    float greatPercentageValue =0f;
    float perfectPercentageValue = 0f;
    int goodCount = 0;
    int greatCount = 0;
    int perfectCount = 0;
    int CPCount = 0;
    int missCount = 0;

    public float fadeTime = 3f;
    public GameObject fadeOutPanel = null;
    public Camera mainCamera;

    #region EnlargeVariable
    // those variable will affect the enlarge of custom box creating
    [SerializeField] float enlargeTime;
    protected float enlargeRate ; // Rate of enlargement per second
    #endregion
    #region Variable
    //variable that will define each object for each box
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

    private Renderer rendererGood;
    private Renderer rendererGreat;
    private Renderer rendererPerfect;
    private Renderer rendererCP;
    private Color initialColorGood;
    private Color initialColorGreat;
    private Color initialColorPerfect;
    private Color initialColorCP;
    private Color transparentColorGood;
    private Color transparentColorGreat;
    private Color transparentColorPerfect;
    private Color transparentColorCP;

    private string beatmapName="";
    private string imagePath="";
    private int diff = 0;
    private string Artist = "";
    private string filePath = "";
    private string songPath = "";


    void Start()
    {
        // related to EnlargeObject
        enlargeRate = 100 / enlargeTime;

        for (int i = 0; i <= 25; i++)
        {
            GameObject ForWordObject = buttonaaa[i];
            Transform GoodText = ForWordObject.transform.Find("Good");
            Transform GreatText = ForWordObject.transform.Find("Great");
            Transform PerfectText = ForWordObject.transform.Find("Perfect");
            Transform CPText = ForWordObject.transform.Find("Critical Perfect");
            Transform MissText = ForWordObject.transform.Find("Miss");

            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 0);
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 0);
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 0);
            CPText.GetComponent<TextMeshProUGUI>().color = new Color32(253, 138, 51, 0);
            MissText.GetComponent<TextMeshProUGUI>().color = new Color32(149, 140, 142, 0);

            UnityEngine.Debug.Log("GoodText: " + GoodText);
            UnityEngine.Debug.Log("GreatText: " + GreatText);
            UnityEngine.Debug.Log("PerfectText: " + PerfectText);
            UnityEngine.Debug.Log("CPText: " + CPText);
        }

        UnityEngine.Debug.ClearDeveloperConsole();
        startTime = Time.time;
        isStarted = false;





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

        filePath = readFile.getMapPath1().ToString();
        imagePath = readFile.getImagePath1().ToString();
        songPath = readFile.GetSongPath1().ToString();

        SceneManager.UnloadSceneAsync("MainLevelScene");


       




    }




    private void Awake()
    {
        for (int i = 0; i <= 25; i++)
        {
            GameObject ForWordObject = buttonaaa[i];
            Transform GoodText = ForWordObject.transform.Find("Good");
            Transform GreatText = ForWordObject.transform.Find("Great");
            Transform PerfectText = ForWordObject.transform.Find("Perfect");
            Transform CPText = ForWordObject.transform.Find("Critical Perfect");
            Transform MissText = ForWordObject.transform.Find("Miss");

            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 255);
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 255);
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 255);
            CPText.GetComponent<TextMeshProUGUI>().color = new Color32(253, 138, 51, 255);
            MissText.GetComponent<TextMeshProUGUI>().color = new Color32(149, 140, 142, 255);

            UnityEngine.Debug.Log("GoodText: " + GoodText);
            UnityEngine.Debug.Log("GreatText: " + GreatText);
            UnityEngine.Debug.Log("PerfectText: " + PerfectText);
            UnityEngine.Debug.Log("CPText: " + CPText);
            UnityEngine.Debug.Log("MissText: " + MissText);
        }
    }
    private void FixedUpdate()
    {

        if (!isStarted)
        {
            
            UnityEngine.Debug.Log(imagePath);

            string beatmapDirectory = Path.GetDirectoryName(filePath); // gets the directory path of the beatmap
            beatmapName = new DirectoryInfo(beatmapDirectory).Name; // gets the name of the beatmap directory
            UnityEngine.Debug.Log(beatmapName);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                UnityEngine.Debug.LogError("File not found at: " + filePath);
                return;
            }

            // Read the file and log each line
            string[] lines = File.ReadAllLines(filePath);




            // Read the map info
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.StartsWith("[MAP INFO]"))
                    {
                        i++;
                        while (i < lines.Length && !lines[i].StartsWith("["))
                        {
                            string[] parts = lines[i].Split(':');
                            if (parts.Length == 2 && parts[0].Trim() == "Diff")
                            {
                                int.TryParse(parts[1].Trim(), out diff);
                                UnityEngine.Debug.Log($"{i} {diff}");
                            }
                            if (parts.Length == 2 && parts[0].Trim() == "Artist")
                            {
                                Artist = parts[1].Trim();
                                UnityEngine.Debug.Log($"{i} {Artist}");
                            }
                        i++;
                        }
                        break;
                    }
                }

            // Read map data
            for (int x = 0; x < lines.Length; x++)
            {
                string line = lines[x];
                if (line.StartsWith("[MAP DATA]"))
                {
                    x++;
                    while (x < lines.Length && lines[x].StartsWith("["))
                    {
                        
                        UnityEngine.Debug.Log(line);

                        string[] splitLine = lines[x].Split('['); // split the line at each '[' character
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
                                        returnButtonType(getkey, out convertKey, out isAir);

                                        //UnityEngine.Debug.Log("log time la: " + logTime);  // debug log thoi gian can de in ra dong lenh phia sau



                                        StartCoroutine(EnlargeObject(logTime - enlargeTime, convertKey, isAir)); // bat dau enlarge object vs 1 chut offset ( logtime - enlargetime )
                                        if (getkey.Length == 1)
                                        {
                                            int needshiftnum = 0;
                                            if (getkey.Length == 1 && char.IsUpper(getkey[0]))
                                            {
                                                getkey = char.ToLower(getkey[0]).ToString();
                                                needshiftnum = 1;
                                            }
                                            if (needshiftnum == 1)
                                            {
                                                notes.Add(new Note { time = logTime, key = getkey, NeedShift = true, NoteStatus = false, });
                                                numOfNotes++;
                                            }
                                            if (needshiftnum == 0)
                                            {
                                                notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false, });
                                                numOfNotes++;
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
                                        int convertKey;
                                        bool isAir = false;
                                        returnButtonType(getkey, out convertKey, out isAir);


                                        //UnityEngine.Debug.Log("log time la: " + logTime);
                                        StartCoroutine(EnlargeObject(logTime - enlargeTime, convertKey, isAir));
                                        int needshiftnum = 0;
                                        if (getkey.Length == 1 && char.IsUpper(getkey[0]))
                                        {
                                            getkey = char.ToLower(getkey[0]).ToString();
                                            needshiftnum = 1;
                                        }
                                        if (needshiftnum == 1)
                                        {
                                            notes.Add(new Note { time = logTime, key = getkey, NeedShift = true, NoteStatus = false, });
                                            numOfNotes++;
                                        }
                                        if (needshiftnum == 0)
                                        {
                                            notes.Add(new Note { time = logTime, key = getkey, NeedShift = false, NoteStatus = false, });
                                            numOfNotes++;
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
                        x++;
                    }
                    break;
                }
            }
            perfectPercentageValue = 100f / numOfNotes;
            greatPercentageValue = 0.75f * perfectPercentageValue;
            goodPercentageValue = 0.5f * perfectPercentageValue;


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
                convertkey = 3;
                isAir = false;
                break;

            case "t":
                //EnlargeObject = CreateObjectT;
                convertkey = 4;
                isAir = false;
                break;

            case "y":
                //EnlargeObject = CreateObjectY;
                convertkey = 5;
                isAir = false;
                break;

            case "u":
                convertkey = 6;
                isAir = false;
                //EnlargeObject = CreateObjectU;
                break;

            case "i":
                //EnlargeObject = CreateObjectI;
                convertkey = 7;
                isAir = false;
                break;

            case "o":
                // EnlargeObject = CreateObjectO;
                convertkey = 8;
                isAir = false;
                break;

            case "p":
                // EnlargeObject = CreateObjectP;
                convertkey = 9;
                isAir = false;
                break;

            case "a":
                //  EnlargeObject = CreateObjectA;
                convertkey = 10;
                isAir = false;
                break;

            case "s":
                //  EnlargeObject = CreateObjectS;
                convertkey = 11;
                isAir = false;
                break;

            case "d":
                //   EnlargeObject = CreateObjectD;
                convertkey = 12;
                isAir = false;
                break;

            case "f":
                //  EnlargeObject = CreateObjectF;
                convertkey = 13;
                isAir = false;
                break;

            case "g":
                //  EnlargeObject = CreateObjectG;
                convertkey = 14;
                isAir = false;
                break;

            case "h":
                //    EnlargeObject = CreateObjectH;
                convertkey = 15;
                isAir = false;
                break;

            case "j":
                //  EnlargeObject = CreateObjectJ;
                convertkey = 16;
                isAir = false;
                break;

            case "k":
                //  EnlargeObject = CreateObjectK;
                convertkey = 17;
                isAir = false;
                break;

            case "l":
                //  EnlargeObject = CreateObjectL;
                convertkey = 18;
                isAir = false;
                break;

            case "z":
                //    EnlargeObject = CreateObjectZ;
                convertkey = 19;
                isAir = false;
                break;

            case "x":
                //    EnlargeObject = CreateObjectX;
                convertkey = 20;
                isAir = false;
                break;

            case "c":
                //      EnlargeObject = CreateObjectC;
                convertkey = 21;
                isAir = false;
                break;

            case "v":
                //  EnlargeObject = CreateObjectV;
                convertkey = 22;
                isAir = false;
                break;

            case "b":
                //   EnlargeObject = CreateObjectB;
                convertkey = 23;
                isAir = false;
                break;

            case "n":
                //   EnlargeObject = CreateObjectN;
                convertkey = 24;
                isAir = false;
                break;

            case "m":
                //     EnlargeObject = CreateObjectM;
                convertkey = 25;
                isAir = false;
                break;

            case "Q":
                //   EnlargeObject = CreateObjectAirQ;
                convertkey = 0;
                isAir = true;
                break;

            case "W":
                //    EnlargeObject = CreateObjectAirW;
                convertkey = 1;
                isAir = true;
                break;

            case "E":
                //   EnlargeObject = CreateObjectAirE;
                convertkey = 2;
                isAir = true;
                break;

            case "R":
                //    EnlargeObject = CreateObjectAirR;
                convertkey = 3;
                isAir = true;
                break;

            case "T":
                //    EnlargeObject = CreateObjectAirT;
                convertkey = 4;
                isAir = true;
                break;

            case "Y":
                //  EnlargeObject = CreateObjectAirY;
                convertkey = 5;
                isAir = true;
                break;

            case "U":
                //   EnlargeObject = CreateObjectAirU;
                convertkey = 6;
                isAir = true;
                break;

            case "I":
                //  EnlargeObject = CreateObjectAirI;
                convertkey = 7;
                isAir = true;
                break;

            case "O":
                //   EnlargeObject = CreateObjectAirO;
                convertkey = 8;
                isAir = true;
                break;

            case "P":
                //   EnlargeObject = CreateObjectAirP;
                convertkey = 9;
                isAir = true;
                break;

            case "A":
                //   EnlargeObject = CreateObjectAirA;
                convertkey = 10;
                isAir = true;
                break;

            case "S":
                //   EnlargeObject = CreateObjectAirS;
                convertkey = 11;
                isAir = true;
                break;

            case "D":
                //  EnlargeObject = CreateObjectAirD;
                convertkey = 12;
                isAir = true;
                break;

            case "F":
                //   EnlargeObject = CreateObjectAirF;
                convertkey = 13;
                isAir = true;
                break;

            case "G":
                //   EnlargeObject = CreateObjectAirG;
                convertkey = 14;
                isAir = true;
                break;

            case "H":
                //   EnlargeObject = CreateObjectAirH;
                convertkey = 15;
                isAir = true;
                break;

            case "J":
                //    EnlargeObject = CreateObjectAirJ;
                convertkey = 16;
                isAir = true;
                break;

            case "K":
                //   EnlargeObject = CreateObjectAirK;
                convertkey = 17;
                isAir = true;
                break;

            case "L":
                //   EnlargeObject = CreateObjectAirL;
                convertkey = 18;
                isAir = true;
                break;

            case "ZZ":
                //   EnlargeObject = CreateObjectAirZ;
                convertkey = 19;
                isAir = true;
                break;

            case "X":
                //  EnlargeObject = CreateObjectAirX;
                convertkey = 20;
                isAir = true;
                break;

            case "C":
                //  EnlargeObject = CreateObjectAirC;
                convertkey = 21;
                isAir = true;
                break;

            case "V":
                //  EnlargeObject = CreateObjectAirV;
                convertkey = 22;
                isAir = true;
                break;

            case "B":
                //    EnlargeObject = CreateObjectAirB;
                convertkey = 23;
                isAir = true;
                break;

            case "N":
                //    EnlargeObject = CreateObjectAirN;
                convertkey = 24;
                isAir = true;
                break;

            case "M":
                //  EnlargeObject = CreateObjectAirM;
                convertkey = 25;
                isAir = true;
                break;

            default:
                throw new ArgumentException("Invalid key: " + key);
        }
        return 0; // Change this to the appropriate value for your method
    }
    private System.Collections.IEnumerator LogAtTime(float time, string message)
    {
        while (Time.time < time)
        {
            yield return null;
        }

        
        UnityEngine.Debug.Log(message + " " + time);
    }

    IEnumerator EnlargeObject(float triggerTime, int convertKey, bool isAir)
    {
        GameObject EnlargeObject = buttonaaa[convertKey];
        Transform EnlargeGroundObject = EnlargeObject.transform.Find("SquareInput");
        Transform EnlargeAirObject = EnlargeObject.transform.Find("SquareAir");


        while (Time.time < triggerTime)
        {
            yield return null;
        }
        float startTime = triggerTime;
        UnityEngine.Debug.Log("start time la: " + startTime);
        
        float endTime = triggerTime + enlargeTime;
        UnityEngine.Debug.Log("enlarge time la: " + enlargeTime);
        UnityEngine.Debug.Log("end time la: " + endTime);
        if (isAir==false)
        {
            while (Time.time < endTime)
            {

                float timeElapsed = Time.time - startTime;
                float scale = timeElapsed * enlargeRate;
                EnlargeGroundObject.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
        if (isAir == true)
        {
            while (Time.time < endTime)
            {

                float timeElapsed = Time.time - startTime;
                float scale = timeElapsed * enlargeRate;
                EnlargeAirObject.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
        UnityEngine.Debug.Log(Time.time + " Da hoan thanh xong ");
        EnlargeGroundObject.transform.localScale = new Vector3(0f, 0f, 0f);
        EnlargeAirObject.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    IEnumerator ListNote(List<Note> notes)
    {

        for (int i =0;i< notes.Count; i++)
        {
            Note note = notes[i];

            int convertKey;
            bool isAir = false; //not need
            returnButtonType(note.key, out convertKey, out isAir);

            yield return StartCoroutine(CreateNote(note.time-0.1f, note.key, note.NeedShift, convertKey));           
        }

        EndNoteTime = notes[notes.Count-1].time;



        UnityEngine.Debug.Log("end node time la: " + EndNoteTime);
        Invoke("LowerVolumeAndEndDelayed", 3f);
        StartCoroutine(FadeOutCamera());
        UnityEngine.Debug.Log("So luong great la: " + greatCount + "\nSo luong good la: " + goodCount + "\nSo luong perfect la: " + perfectCount + "\nSo luong CP la: " + CPCount + "\nPercentage la: " + percentage);
        UnityEngine.Debug.Log("Huy rat chi la cute");
    }


    private void LowerVolumeAndEndDelayed()
    {
        
        StartCoroutine(LowerVolumeAndEnd());
        return;
    }

    IEnumerator LowerVolumeAndEnd()
    {
        float fadeTime = 3f;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
      //  CalculateScore();
      //  ShowResults();
    }




    IEnumerator CreateNote(float Atime, string key, bool needshift, int convertKey)
    {




        //UnityEngine.Debug.Log("key la: " + key);
        while (Time.time < Atime) //active time
        {
            yield return null;
        }


        float startTime = Atime ;
        UnityEngine.Debug.Log("start time create note la: " + startTime);
        float endtime = Atime + 0.2f;
        UnityEngine.Debug.Log("start time end note la: " + endtime);


        if (needshift == false)
        {
            while (true) // runs continuously until the coroutine is stopped
            {
                if (Time.time >= startTime && Time.time < endtime) // check if Time.time is within the desired range
                {
                    float timeelapsed = Time.time - startTime;
                    if (Input.GetKey(key))
                    {
                        LogResultClick(timeelapsed, convertKey);
                        yield break; 
                    }
                }
                else if (Time.time >= endtime) // check if Time.time has exceeded the endtime
                {
                    UnityEngine.Debug.Log("Goodbye");
                    float timeelapsed = Time.time - startTime;
                    LogResultClick(timeelapsed, convertKey);
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
                        LogResultClick(timeelapsed, convertKey);
                        yield break;
                    }
                }
                else if (Time.time >= endtime) // check if Time.time has exceeded the endtime
                {
                    UnityEngine.Debug.Log("Goodbye");
                    float timeelapsed = Time.time - startTime;
                    LogResultClick(timeelapsed, convertKey);

                    yield break; // exit the coroutine
                }
                yield return null;
            }
        }


    }
    // tuong tu enlarge object but se tao ra cac time delay de bam


    void LogResultClick (float timeelapsed, int convertKey)
    {


        GameObject ForWordObject = buttonaaa[convertKey];
        Transform GoodText = ForWordObject.transform.Find("Good");
        Transform GreatText = ForWordObject.transform.Find("Great");
        Transform PerfectText = ForWordObject.transform.Find("Perfect");
        Transform CPText = ForWordObject.transform.Find("Critical Perfect");
        Transform MissText = ForWordObject.transform.Find("Miss");


        UnityEngine.Debug.Log("Ban da click vao luc: " + timeelapsed);
        if (timeelapsed <= 0.03f && timeelapsed >= 0)
        {
            UnityEngine.Debug.Log("Good");
            percentage += goodPercentageValue;
            goodCount++;
            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 255);
            StartCoroutine(FadeOutText(GoodText, 2f));

        }
        else if (timeelapsed <= 0.06f)
        {
            UnityEngine.Debug.Log("Great");
            percentage += greatPercentageValue;
            greatCount++;
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 255);
            StartCoroutine(FadeOutText(GreatText, 2f));
        }
        else if (timeelapsed <= 0.08f)
        {
            UnityEngine.Debug.Log("Perfect");
            percentage += perfectPercentageValue;
            perfectCount++;
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 255);
            StartCoroutine(FadeOutText(PerfectText, 2f));
        }
        else if (timeelapsed <= 0.12)
        {
            UnityEngine.Debug.Log("CriticalPerfect");
            percentage += perfectPercentageValue;
            CPCount++;
            CPText.GetComponent<TextMeshProUGUI>().color = new Color32(253, 138, 51, 255);
            StartCoroutine(FadeOutText(CPText, 2f));
        }
        else if (timeelapsed <= 0.14)
        {
            UnityEngine.Debug.Log("Perfect");
            percentage += perfectPercentageValue;
            perfectCount++;
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 255);
            StartCoroutine(FadeOutText(PerfectText, 2f));
        }
        else if (timeelapsed <= 0.17)
        {
            UnityEngine.Debug.Log("Great");
            percentage += greatPercentageValue;
            greatCount++;
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 255);
            StartCoroutine(FadeOutText(GreatText, 2f));
        }
        else if (timeelapsed <= 0.20)
        {
            UnityEngine.Debug.Log("Good");
            percentage += goodPercentageValue;
            goodCount++;
            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 255);
            StartCoroutine(FadeOutText(GoodText, 2f));
        }
        else if (timeelapsed >0.2)
        {
            UnityEngine.Debug.Log("Miss");
            missCount++;
            MissText.GetComponent<TextMeshProUGUI>().color = new Color32(93, 88, 89, 255);
            StartCoroutine(FadeOutText(MissText, 2f));
        }
    }

    IEnumerator FadeOutText(Transform textTransform, float duration)
    {
        TextMeshProUGUI textMesh = textTransform.GetComponent<TextMeshProUGUI>();

        Color initialColor = textMesh.color;
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            textMesh.color = Color.Lerp(initialColor, transparentColor, t);
            // Wait for the next frame
            yield return null;
        }
    }



    IEnumerator FadeOutCamera()
    {
        // Fade out the camera
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeTime);
            mainCamera.backgroundColor = Color.Lerp(Color.white, Color.black, normalizedTime);
            yield return null;
        }

        // Debug statement to check if we made it past the camera fade
        UnityEngine.Debug.Log("Camera fade finished");

        // Create the fade out panel
        GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        if (panel == null)
        {
            UnityEngine.Debug.LogError("Failed to instantiate fadeOutPanel");
            yield break;
        }

        RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
        panelRectTransform.SetParent(mainCamera.transform, false);
        panelRectTransform.anchorMin = new Vector2(0, 0);
        panelRectTransform.anchorMax = new Vector2(1, 1);
        panelRectTransform.anchoredPosition = Vector2.zero;
        panelRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // Debug statement to check if the fade out panel was created
        UnityEngine.Debug.Log("Fade out panel created");

        // Load the new scene
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("ScoreShow", LoadSceneMode.Additive);

        // Destroy the fade out panel
        Destroy(panel);

        // Debug statement to check if we made it to the end of the coroutine
        UnityEngine.Debug.Log("FadeOutCamera coroutine finished");
    }

    public int GetCPCount ()
    {
        return CPCount;
    }

    public int GetPerfectCount()
    {
        return perfectCount;
    }

    public int GetGoodCount()
    {
        return goodCount;
    }

    public int GetGreatCount ()
    {
        return greatCount;
    }

    public int GetMissCount ()
    {
        return missCount;
    }

    public float GetPercentage ()
    {
        return percentage;
    }

    public string GetMapName ()
    {
        return beatmapName;
    }
    
    public string GetImagePath ()
    {
        return imagePath;
    }

    public int GetDiff()
    {
        return diff;
    }

    public string GetArtistText ()
    {
        return Artist;
    }
    // create many object at the sameplace




}

