using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEditor.Experimental.GraphView;


public class ReadFile : MonoBehaviourPunCallbacks
{
    private float startTime; // time when the script starts
    private bool isStarted; // whether the script has started reading the file
                            //Unity.VisualScripting.Timer t = new Unity.VisualScripting.Timer();
    float EndNoteTime = 0f;

    public AudioSource audioSource;


    //for score variable
    int numOfNotes = 0;
    public static float percentage = 0f;
    float goodPercentageValue = 0f;
    float greatPercentageValue = 0f;
    float perfectPercentageValue = 0f;
    int goodCount = 0;
    int greatCount = 0;
    int perfectCount = 0;
    int CPCount = 0;
    int missCount = 0;

    public GameObject FailCanvas;
    public GameObject FailButton;


    public float fadeTime = 3f;

    public Camera mainCamera;

    public float timedelaybeforetractstart = 0f;

    #region EnlargeVariable
    // those variable will affect the enlarge of custom box creating
    //[SerializeField] float enlargeTime;
    protected float enlargeRate; // Rate of enlargement per second
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

    private struct Visual
    {
        public float time;
        public float r;
        public float g;
        public float b;
        public float o;
    }

    List<Visual> visuals = new List<Visual>();

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

    private string beatmapName = "";
    private string imagePath ;
    private int diff = 0;
    private string HpString = "";
    private float HpValue = 0f;
    private string Artist = "";
    private string filePath ;
    public static string scorePath ;
    public static string songPath ;
    private string BPMText = "";
    private float BPMValue = 0;

    private float OffsetValueSetting = 0f;
    private float ARValueSetting = 0f;
    private int BGOpacitySetting = 0;

    public Animator transitionAnim2;

    public Image hpBarImage;
    private bool stopHpBarReduction = false;

    public Image backgroundImage;

    public TextMeshProUGUI ScoreNow;
    public TextMeshProUGUI ComboNow;
    int combo = 0;


    public static bool isMulti;
    string idForMulti = "";

    void Start()
    {

        combo = 0;
        ComboNow.text=combo.ToString();
        FailButton.SetActive(false);
        FailCanvas.SetActive(false);

 


        transitionAnim2.SetBool("isFadeIn", false);


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

            //UnityEngine.Debug.Log("GoodText: " + GoodText);
            //UnityEngine.Debug.Log("GreatText: " + GreatText);
            //UnityEngine.Debug.Log("PerfectText: " + PerfectText);
            //UnityEngine.Debug.Log("CPText: " + CPText);
        }

        UnityEngine.Debug.ClearDeveloperConsole();
        startTime = Time.time;
        isStarted = false;



        OffsetValueSetting = LoadBeatmapList.OffsetValue;
        ARValueSetting = LoadBeatmapList.ARValue;
        BGOpacitySetting = LoadBeatmapList.BgOpacityValue;
        Debug.Log("ar value setting la: " + ARValueSetting);

        //UnityEngine.Debug.Log("Offset: " + OffsetValueSetting);
        //UnityEngine.Debug.Log("Ar: " + ARValueSetting);
        //UnityEngine.Debug.Log("Image path: " + imagePath);

        // related to EnlargeObject
        enlargeRate = 1 / ARValueSetting;
        LoadImageFromFile(imagePath);

    }




    private void Awake()
    {

        percentage = 0f;
        if (isMulti == true) //because multi use sync for data song so we can't just use directory from another user
        {
            //Debug.Log("huy ultra cute, the ismulti is true");
            string pathforMulti = Application.dataPath + "\\Game_data\\Beatmaps";
            idForMulti = LoadBeatmapList.IDSong;
            string folderName = GetFolderNameByID(idForMulti);
            if (!string.IsNullOrEmpty(folderName))
            {
                //Debug.Log("Folder with ID " + idForMulti + ": " + folderName);
            }
            else
            {
                //Debug.Log("Folder with ID " + idForMulti + " not found.");
            }

            string folderPathForMulti = Path.Combine(pathforMulti, folderName);
            filePath = Path.Combine(folderPathForMulti, "map.txt");
            scorePath = Path.Combine(folderPathForMulti, "score.txt");
            imagePath = Path.Combine(folderPathForMulti, "bg.jpg");
            songPath = Path.Combine(folderPathForMulti, "audio.mp3");

            //Debug.Log("filepath multi la: " + filePath);
            //Debug.Log("scorepath multi la: " + scorePath);
            //Debug.Log("imagepath multi la: " + imagePath);
            //Debug.Log("songpath multi la: " + songPath);

        }
        if (isMulti == false)
        {
            //Load info of beatmap
            filePath = LoadBeatmapList.infoPath;
            scorePath = LoadBeatmapList.scorePath;
            imagePath = LoadBeatmapList.imagePath;
            songPath = LoadBeatmapList.songPath;

        }
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

            // UnityEngine.Debug.Log("GoodText: " + GoodText);
            // UnityEngine.Debug.Log("GreatText: " + GreatText);
            // UnityEngine.Debug.Log("PerfectText: " + PerfectText);
            // UnityEngine.Debug.Log("CPText: " + CPText);
            //  UnityEngine.Debug.Log("MissText: " + MissText);
        }
    }
    private void FixedUpdate()
    {

        if (!isStarted)
        {

            //UnityEngine.Debug.Log(imagePath);
            //UnityEngine.Debug.Log(filePath);

            string beatmapDirectory = Path.GetDirectoryName(filePath); // gets the directory path of the beatmap
 
            beatmapName = new DirectoryInfo(beatmapDirectory).Name; // gets the name of the beatmap directory
            //UnityEngine.Debug.Log(beatmapName);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                //    UnityEngine.Debug.LogError("File not found at: " + filePath);
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
                            //        UnityEngine.Debug.Log($"{i} {diff}");
                        }
                        if (parts.Length == 2 && parts[0].Trim() == "Artist")
                        {
                            Artist = parts[1].Trim();
                            //          UnityEngine.Debug.Log($"{i} {Artist}");
                        }
                        if (parts.Length == 2 && parts[0].Trim() == "Hp")
                        {
                            HpString = parts[1].Trim();
                            //          UnityEngine.Debug.Log($"{i} {Artist}");
                        }
                        if (float.TryParse(HpString, out float Hp))
                        {
                            HpValue = Hp;
                            //UnityEngine.Debug.Log("Hp value: " + HpValue);
                        }
                        if (parts.Length == 2 && parts[0].Trim() == "BPM")
                        {
                            BPMText = parts[1].Trim();
                            //          UnityEngine.Debug.Log($"{i} {Artist}");
                        }
                        if (float.TryParse(BPMText, out float result))
                        {
                            BPMValue = result;
                            //UnityEngine.Debug.Log("BPM value: " + BPMValue);
                        }
                        i++;
                    }
                    break;
                }
            }

            StartCoroutine(StartHpBarReduction(HpValue));

            // Read map visual
            for (int x = 0; x < lines.Length; x++)
            {
                string line = lines[x];
                


                // Check if the line starts with "[MAP VISUAL]"
                if (line.StartsWith("[MAP VISUAL]"))
                {
                    Debug.Log("map visual detected");
                    x++; // Move to the next line                   
                    Debug.Log("line length la: " + lines.Length + " x la: " + x);
                    while (x < lines.Length && lines[x].StartsWith("["))
                    {


                        
                        string[] splitLine = lines[x].Substring(1, lines[x].Length - 2).Split(new[] { "][" }, StringSplitOptions.None);


                        Debug.Log("split line 0: " + splitLine[0]);
                        Debug.Log("split line 1: " + splitLine[1]);
                        Debug.Log("split line length : " + splitLine.Length);


                        if (splitLine.Length >= 2)
                        {
                            Debug.Log("lets go");
                            float time;
                            float.TryParse(splitLine[0], out time);



                            string[] colorValues = splitLine[1].Split(',');
                            float r, g, b, o;
                            float.TryParse(colorValues[0], out r);
                            float.TryParse(colorValues[1], out g);
                            float.TryParse(colorValues[2], out b);
                            float.TryParse(colorValues[3], out o);

                            Debug.Log(time + " " + r + " " + g + " " + b + " " + o);
                            // Trigger the color change at the specified time
                            visuals.Add(new Visual { time = time, r=r, b=b, g=g, o=o});
                            
                        }

                        x++; // Move to the next line
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
                                                                                                            if (line.StartsWith("[MAP VISUAL]"))
                                                                                                            {
                                                                                                                  break;
                                                                                                            }
                        x++;
                    while (x < lines.Length && lines[x].StartsWith("["))
                    {

                        //    UnityEngine.Debug.Log(line);

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



                                        StartCoroutine(EnlargeObject(logTime - ARValueSetting + timedelaybeforetractstart + 0.36f + (60 / BPMValue) * 4 + OffsetValueSetting, convertKey, isAir)); // bat dau enlarge object vs 1 chut offset ( logtime - enlargetime )
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
                                                        //    UnityEngine.Debug.Log(getkey);


                                    if (i == key.Length - 1)
                                    {
                                        int convertKey;
                                        bool isAir = false;
                                        returnButtonType(getkey, out convertKey, out isAir);


                                        //UnityEngine.Debug.Log("log time la: " + logTime);
                                        StartCoroutine(EnlargeObject(logTime - ARValueSetting + timedelaybeforetractstart + 0.36f + (60 / BPMValue) * 4 + OffsetValueSetting, convertKey, isAir));
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

            StartCoroutine(ListVisual(visuals));
            StartCoroutine(ListNote(notes));

        }

        isStarted = true;

    }

    int returnButtonType(string key, out int convertkey, out bool isAir)
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
        GameObject originalObject = buttonaaa[convertKey];
        Transform originalGroundObject = originalObject.transform.Find("SquareInput");
        Transform originalAirObject = originalObject.transform.Find("SquareAir");
        //Debug.Log("EnlargeObject coroutine started.");
        while (Time.time < triggerTime)
        {
            //Debug.Log("Waiting for trigger time. Current time: " + Time.time + ", Trigger time: " + triggerTime);
            yield return null;
        }

        float startTime = triggerTime;
        float endTime = triggerTime + ARValueSetting;

        if (isAir == false)
        {
            Transform clonedGroundObject = Instantiate(originalGroundObject, originalObject.transform);
            //Debug.Log("Cloned Object: " + clonedGroundObject.name);
            while (Time.time < endTime)
            {
                float timeElapsed = Time.time - startTime;
                float scale = timeElapsed * enlargeRate;
                clonedGroundObject.localScale = new Vector3(scale, scale, scale);
                //Debug.Log("Cloned Ground Object Position: " + clonedGroundObject.position);
                //Debug.Log("Cloned Ground Object Local Scale: " + clonedGroundObject.localScale);
                yield return null;


            }

            Destroy(clonedGroundObject.gameObject);
        }

        if (isAir == true)
        {
            Transform clonedAirObject = Instantiate(originalAirObject, originalObject.transform);
            //Debug.Log("Cloned Object: " + clonedAirObject.name);
            while (Time.time < endTime)
            {
                float timeElapsed = Time.time - startTime;
                float scale = timeElapsed * enlargeRate;
                clonedAirObject.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }



            Destroy(clonedAirObject.gameObject);
        }

        //originalGroundObject.transform.localScale = new Vector3(0f, 0f, 0f);
        //originalAirObject.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    IEnumerator ListVisual(List<Visual> visualtrigger)
    {
        int countgo = 0;
        float beforetime = 0;
        foreach (Visual visual in visualtrigger)
        {
            float triggerTime = visual.time;
            if (countgo==0)
            {
                beforetime= triggerTime;
                yield return new WaitForSeconds(triggerTime + timedelaybeforetractstart + 0.26f + (60 / BPMValue) * 4 + OffsetValueSetting);
                countgo++;
            }
            if(countgo>=1)
            {
                yield return new WaitForSeconds(triggerTime-beforetime);
                beforetime = triggerTime;
            }
            
            
            StartCoroutine(TriggerColorChange(visual.r, visual.g, visual.b, visual.o));
        }
    }

    IEnumerator ListNote(List<Note> notes)
    {

        for (int i = 0; i < notes.Count; i++)
        {
            Note note = notes[i];

            int convertKey;
            bool isAir = false; //not need
            returnButtonType(note.key, out convertKey, out isAir);


            yield return StartCoroutine(CreateNote(note.time + timedelaybeforetractstart + 0.26f + (60 / BPMValue) * 4 + OffsetValueSetting, note.key, note.NeedShift, convertKey));
        }

        EndNoteTime = notes[notes.Count - 1].time;

        // Stop the HP bar reduction coroutine
        stopHpBarReduction = true;

        // UnityEngine.Debug.Log("end node time la: " + EndNoteTime);
        Invoke("LowerVolumeAndEndDelayed", 3f);

        StartCoroutine(FadeOutCamera());
        //   UnityEngine.Debug.Log("So luong great la: " + greatCount + "\nSo luong good la: " + goodCount + "\nSo luong perfect la: " + perfectCount + "\nSo luong CP la: " + CPCount + "\nPercentage la: " + percentage);
        //   UnityEngine.Debug.Log("Huy rat chi la cute");
    }

    // HP BAR METHOD
    #region
    //------------------------------------------------------------------


    private IEnumerator StartHpBarReduction(float hpValue)
    {
        //Debug.Log("Start HP bar reduction coroutine");
        //Debug.Log("HPVALUE LA: " + hpValue);

        yield return new WaitForSeconds(timedelaybeforetractstart + 0.26f + (60 / BPMValue) * 4 + OffsetValueSetting);

        Debug.Log("HP bar reduction started");

        float initialWidth = hpBarImage.rectTransform.sizeDelta.x;

        while (hpValue > 0f && !stopHpBarReduction)
        {
            float targetWidth = 0;
            float currentWidth = hpBarImage.rectTransform.sizeDelta.x;

            if (currentWidth > targetWidth)
            {
                float reductionAmount = HpValue * 10 * Time.deltaTime;
                float newWidth = Mathf.Max(targetWidth, currentWidth - reductionAmount);
                hpBarImage.rectTransform.sizeDelta = new Vector2(newWidth, hpBarImage.rectTransform.sizeDelta.y);
            }
            if (currentWidth<=targetWidth)
            {
                if(ReadFile.isMulti==false)
                {
                    //Time.timeScale = 0f;
                    AudioListener.pause = true;
                    FailCanvas.SetActive(true);
                    FailButton.SetActive(true);
                }
            }
            //Debug.Log("HP value: " + hpValue + ", Current width: " + hpBarImage.rectTransform.sizeDelta.x + ", Target width: " + targetWidth);
            yield return null;

        }




        Debug.Log("HP bar reduction completed");

        // HP bar reached minimum size, trigger lose notification here
        // ...
    }
    private void PlusHpBar(float hpValue, int type) //type 0 = perfect + cP , 1 = great, 2 = good
    {


        float currentWidth = hpBarImage.rectTransform.sizeDelta.x;
        float plusAmount = 0;

        if (type == 0)
        {
            plusAmount = (11 - hpValue) * 10;
            float newWidth = currentWidth + plusAmount;
            hpBarImage.rectTransform.sizeDelta = new Vector2(newWidth, hpBarImage.rectTransform.sizeDelta.y);
        }
        if (type == 1)
        {
            plusAmount = (11 - hpValue) * 5;
            float newWidth = currentWidth + plusAmount;
            hpBarImage.rectTransform.sizeDelta = new Vector2(newWidth, hpBarImage.rectTransform.sizeDelta.y);
        }
        if (type == 2)
        {
            plusAmount = (11 - hpValue) * 3;
            float newWidth = currentWidth + plusAmount;
            hpBarImage.rectTransform.sizeDelta = new Vector2(newWidth, hpBarImage.rectTransform.sizeDelta.y);
        }
        if (type == 3)
        {
            plusAmount = (11 - hpValue) * -5;
            float newWidth = currentWidth + plusAmount;
            hpBarImage.rectTransform.sizeDelta = new Vector2(newWidth, hpBarImage.rectTransform.sizeDelta.y);
        }
        //Debug.Log("plusAmount: " + plusAmount + ", Current width: " + hpBarImage.rectTransform.sizeDelta.x);

    }

    //------------------------------------------------------------------
    #endregion


    // LOWER VOLUMN OF THE SONG
    #region
    // -----------------------------------------------------------------

    private void LowerVolumeAndEndDelayed()
    {

        StartCoroutine(LowerVolumeAndEnd());
        return;
    }

    IEnumerator LowerVolumeAndEnd()
    {

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

    // -----------------------------------------------------------------
    #endregion


    IEnumerator CreateNote(float Atime, string key, bool needshift, int convertKey)
    {




        //UnityEngine.Debug.Log("key la: " + key);
        while (Time.time < Atime) //active time
        {
            yield return null;
        }


        float startTime = Atime;
        //  UnityEngine.Debug.Log("start time create note la: " + startTime);
        float endtime = Atime + 0.2f;
        //   UnityEngine.Debug.Log("start time end note la: " + endtime);

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
                    //   UnityEngine.Debug.Log("Goodbye");
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
                    if (Input.GetKey(key) && isShiftPressed == true)
                    {
                        LogResultClick(timeelapsed, convertKey);
                        yield break;
                    }
                }
                else if (Time.time >= endtime) // check if Time.time has exceeded the endtime
                {
                    //   UnityEngine.Debug.Log("Goodbye");
                    float timeelapsed = Time.time - startTime;
                    LogResultClick(timeelapsed, convertKey);

                    yield break; // exit the coroutine
                }
                yield return null;
            }
        }


    }

    // tuong tu enlarge object but se tao ra cac time delay de bam

    // create Log base on result
    #region
    void LogResultClick(float timeelapsed, int convertKey)
    {


        GameObject ForWordObject = buttonaaa[convertKey];
        Transform GoodText = ForWordObject.transform.Find("Good");
        Transform GreatText = ForWordObject.transform.Find("Great");
        Transform PerfectText = ForWordObject.transform.Find("Perfect");
        Transform CPText = ForWordObject.transform.Find("Critical Perfect");
        Transform MissText = ForWordObject.transform.Find("Miss");


        //   UnityEngine.Debug.Log("Ban da click vao luc: " + timeelapsed);
        if (timeelapsed <= 0.03f && timeelapsed >= 0)
        {
            UnityEngine.Debug.Log("Good");
            percentage += goodPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text=combo.ToString();
            goodCount++;
            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 255);
            PlusHpBar(HpValue, 2);
            StartCoroutine(FadeOutText(GoodText, 2f));

        }
        else if (timeelapsed <= 0.06f)
        {
            //   UnityEngine.Debug.Log("Great");
            percentage += greatPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            greatCount++;
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 255);
            PlusHpBar(HpValue, 1);
            StartCoroutine(FadeOutText(GreatText, 2f));
        }
        else if (timeelapsed <= 0.08f)
        {
            UnityEngine.Debug.Log("Perfect");
            percentage += perfectPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            perfectCount++;
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 255);
            PlusHpBar(HpValue, 0);
            StartCoroutine(FadeOutText(PerfectText, 2f));
        }
        else if (timeelapsed <= 0.12)
        {
            UnityEngine.Debug.Log("CriticalPerfect");
            percentage += perfectPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            CPCount++;
            CPText.GetComponent<TextMeshProUGUI>().color = new Color32(253, 138, 51, 255);
            PlusHpBar(HpValue, 0);
            StartCoroutine(FadeOutText(CPText, 2f));
        }
        else if (timeelapsed <= 0.14)
        {
            UnityEngine.Debug.Log("Perfect");
            percentage += perfectPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            perfectCount++;
            PerfectText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 57, 255);
            PlusHpBar(HpValue, 0);
            StartCoroutine(FadeOutText(PerfectText, 2f));
        }
        else if (timeelapsed <= 0.17)
        {
            UnityEngine.Debug.Log("Great");
            percentage += greatPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            greatCount++;
            GreatText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 120, 110, 255);
            PlusHpBar(HpValue, 1);
            StartCoroutine(FadeOutText(GreatText, 2f));
        }
        else if (timeelapsed <= 0.20)
        {
            UnityEngine.Debug.Log("Good");
            percentage += goodPercentageValue;
            ScoreNow.text = percentage.ToString();
            combo++;
            ComboNow.text = combo.ToString();
            goodCount++;
            GoodText.GetComponent<TextMeshProUGUI>().color = new Color32(167, 239, 62, 255);
            PlusHpBar(HpValue, 2);
            StartCoroutine(FadeOutText(GoodText, 2f));
        }
        else if (timeelapsed > 0.2)
        {
            UnityEngine.Debug.Log("Miss");
            missCount++;
            combo=0;
            ComboNow.text = combo.ToString();
            MissText.GetComponent<TextMeshProUGUI>().color = new Color32(93, 88, 89, 255);
            PlusHpBar(HpValue, 3);
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
    #endregion


    IEnumerator FadeOutCamera()
    {

        transitionAnim2.SetBool("isFadeIn", true);

        UnityEngine.Debug.Log("isFadeIn parameter value: " + transitionAnim2.GetBool("isFadeIn"));
        transitionAnim2.SetTrigger("FadeOut");


        if(isMulti==true)
        {
            
        }

        // Load the new scene
        yield return new WaitForSeconds(6f);
        
            SceneManager.LoadScene("ScoreShow", LoadSceneMode.Additive);
        
        



        // Debug statement to check if we made it to the end of the coroutine
        UnityEngine.Debug.Log("FadeOutCamera coroutine finished");
    }

    public int GetCPCount()
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

    public int GetGreatCount()
    {
        return greatCount;
    }

    public int GetMissCount()
    {
        return missCount;
    }

    public float GetPercentage()
    {
        return percentage;
    }


    // create many object at the sameplace

    private void LoadImageFromFile(string path)
    {
        // Load the image file as bytes
        byte[] imageData = System.IO.File.ReadAllBytes(path);

        // Create a new Texture2D
        Texture2D texture = new Texture2D(2, 2);

        // Load the image data into the Texture2D
        texture.LoadImage(imageData);

        // Set the Texture2D as the sprite of the Image component
        backgroundImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    public string GetFolderNameByID(string id)
    {
        string[] folders = Directory.GetDirectories(Application.dataPath + "\\Game_data\\Beatmaps");

        foreach (string folderPath in folders)
        {
            string folderName = Path.GetFileName(folderPath);
            string[] nameParts = folderName.Split('-');

            if (nameParts.Length >= 2)
            {
                string folderID = nameParts[0].Trim();
                string folderTitle = string.Join("-", nameParts, 1, nameParts.Length - 1).Trim();

                if (folderID == id)
                {
                    return folderName;
                }
            }
        }

        return string.Empty; // If folder with the specified ID is not found
    }


    IEnumerator TriggerColorChange(float r, float g, float b, float o)
    {
        Debug.Log("Triggering color change at: " + Time.time);
        yield return StartCoroutine(ChangeColorBackground(r, g, b, o));
    }

    IEnumerator ChangeColorBackground(float r, float g, float b, float o)
    {
        Debug.Log("start changing color at: " + Time.time);
        // Create a new color with the desired RGB values
        Color newColor = new Color(r, g, b, o); // Example: set the color to orange (RGB: 255, 128, 0)

        // Assign the new color to the image
        backgroundImage.color = newColor;
        yield return null;
    }

}





