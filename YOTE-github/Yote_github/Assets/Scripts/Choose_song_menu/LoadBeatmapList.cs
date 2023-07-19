using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class LoadBeatmapList : MonoBehaviourPunCallbacks
{
    public GameObject parentObject;
    public RectTransform scrollViewContent;
    private string path = "";
    [SerializeField] TextMeshProUGUI Title_Song;
    [SerializeField] TextMeshProUGUI DiffText;
    [SerializeField] TextMeshProUGUI Artist;
    [SerializeField] TextMeshProUGUI BPM1;
    [SerializeField] TextMeshProUGUI CreatorText;
    [SerializeField] AudioClip EntrySound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] TextMeshProUGUI SongPathName;

    public static bool isMulti=false;

    private string pathSetting = "";
    public TMP_InputField Offset;
    public TMP_InputField AR;
    public TMP_InputField BG_Opacity;
    public GameObject SettingCanvas;


    public Image imageTitle;
    public Image imageComponent;
    private GameObject parentContainer;

    public static string imagePath = "";
    public static string infoPath = "";
    public static string songPath = "";
    public static string BPMValue = "";
    public static string scorePath = "";
    public static int difficultymap = 0;
    public static string artistmap = "";
    public static string creatormap = "";
    public static float OffsetValue = 0f;
    public static float ARValue = 0f;
    public static int BgOpacityValue = 0;
    public static string IDSong = "";
    public static string ActuallySongName = "";

    public Animator transitionAnim;
    public Animator circle1;
    public Animator circle2;
    public Animator circle3;
    public Animator circle4;
    public string SceneName;


    [SerializeField] GameObject panelfadeout;
    [SerializeField] GameObject circle01;
    [SerializeField] GameObject circle02;
    [SerializeField] GameObject circle03;
    [SerializeField] GameObject circle04;

    public TextMeshProUGUI FolderSongPath;

    public GameObject BackToMenu;
    public GameObject BackToLobby;

    private void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Start()
    {
        if (isMulti==false)
        {
            BackToLobby.SetActive(false);
            BackToMenu.SetActive(true);
        }
        if(isMulti==true)
        {
            BackToLobby.SetActive(true);
            BackToMenu.SetActive(false);

        }

        //Load file setting
        pathSetting = Application.dataPath + "\\Game_data\\settings.txt";
        LoadSettings();


        SettingCanvas.SetActive(false);
        transitionAnim.SetBool("FadeOutOnly", false);
        circle1.SetBool("fadeintrue", false);
        circle2.SetBool("fadeintrue", false);
        circle3.SetBool("fadeintrue", false);
        circle4.SetBool("fadeintrue", false);
        circle01.SetActive(false);
        circle02.SetActive(false);
        circle03.SetActive(false);
        circle04.SetActive(false);
        Debug.Log("FadeOutOnly parameter value: " + transitionAnim.GetBool("FadeOutOnly"));
        StartCoroutine(FadeOutDisable());

        

        //get path
        path = Application.dataPath + "\\Game_data\\Beatmaps";
        string[] folderPaths = Directory.GetDirectories(path);

        //song path tip
        FolderSongPath.text = path;


        foreach (string folderPath in folderPaths)
        {
           

            // Set any necessary properties on the clone object here

            // Get the name of the song (which is the folder name)
            string songName = Path.GetFileName(folderPath);

            // Get the path to the image file
            string imagePath = Path.Combine(folderPath, "bg.jpg");

            // Get the path to the info file
            string infoPath = Path.Combine(folderPath, "map.txt");

            // Get the path to the score file
            string scorePath = Path.Combine(folderPath, "score.txt");

            // Parse the info file to get the artist name and difficulty
            string artistName = "";
            int difficulty = -1;
            float Hp = 0f;
            float BPM = 0f;
            string Creator = "";
            if (File.Exists(infoPath))
            {
                string[] lines = File.ReadAllLines(infoPath);
                foreach (string line in lines)
                {
                    if (line.StartsWith("Artist: "))
                    {
                        artistName = line.Substring("Artist: ".Length).Trim();
                    }
                    else if (line.StartsWith("Hp: "))
                    {
                        float.TryParse(line.Substring("Hp: ".Length).Trim(), out Hp);
                    }
                    else if (line.StartsWith("Diff: "))
                    {
                        int.TryParse(line.Substring("Diff: ".Length).Trim(), out difficulty);
                    }
                    else if (line.StartsWith("BPM: "))
                    {
                        float.TryParse(line.Substring("BPM: ".Length).Trim(), out BPM);
                    }
                    else if (line.StartsWith("Creator: "))
                    {
                        Creator = line.Substring("Creator: ".Length).Trim();
                    }
                }
            }
            string ID = "";
            string actuallySongName = "";
            ReturnNameSong(songName, out actuallySongName,out ID);
            
            Title_Song.text = actuallySongName;
            Artist.text = artistName;
            DiffText.text=difficulty.ToString();
            BPM1.text = BPM.ToString();
            CreatorText.text = Creator;
            SongPathName.text = songName;
            Texture2D textureSmallImage = LoadTextureFromPath(imagePath, 400, 400);
           
            if (textureSmallImage != null) // image for song-bg (400x400)
            {
                Sprite sprite = Sprite.Create(textureSmallImage, new Rect(0, 0, textureSmallImage.width, textureSmallImage.height), Vector2.zero);
                imageTitle.sprite = sprite;
            }
            // Instantiate the clone at the top of the scroll view content
            GameObject clone = Instantiate(parentObject, scrollViewContent);
            parentContainer = clone; // set reference to parent object


            // Attach a Selectable component to the clone
            Selectable selectable = parentContainer.GetComponent<Selectable>();
            EventTrigger eventTrigger = parentContainer.AddComponent<EventTrigger>();

            // Add a pointer click event listener to the EventTrigger component
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => OnPointerClick((PointerEventData)data));
            eventTrigger.triggers.Add(entry);
            UnityEngine.Debug.Log("wtf is going on ?");
        }
    }

    private void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {

        
        Debug.Log("Selected object: " + eventData.pointerCurrentRaycast.gameObject.name);

        // Get the selected song
        GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;

        // Get the parent object of the Song_information that was clicked
        GameObject parentObject = selectedObject.transform.parent.gameObject;

        Debug.Log("parent object: " + parentObject);

        // Get the song name from the title text component on the parent object
        string songName = parentObject.transform.Find("SongPathName").GetComponent<TextMeshProUGUI>().text;
        BPMValue= parentObject.transform.Find("BPM").GetComponent<TextMeshProUGUI>().text;
        string folderPath = Path.Combine(path, songName);
        difficultymap = int.Parse(parentObject.transform.Find("Difficulty").GetComponent<TextMeshProUGUI>().text);
        artistmap = parentObject.transform.Find("Artist_name").GetComponent<TextMeshProUGUI>().text;
        imagePath = Path.Combine(folderPath, "bg.jpg");
        infoPath = Path.Combine(folderPath, "map.txt");
        songPath = Path.Combine(folderPath, "audio.mp3");
        scorePath = Path.Combine(folderPath, "score.txt");
        creatormap = parentObject.transform.Find("Creator").GetComponent<TextMeshProUGUI>().text;

        ReturnNameSong(songName, out ActuallySongName, out IDSong);
        UnityEngine.Debug.Log("ACtually song name: " + ActuallySongName);
        UnityEngine.Debug.Log("Idsong la: " + IDSong);

        if (isMulti==false)
        {
            audioSource.PlayOneShot(EntrySound);
            circle01.SetActive(true);
            circle02.SetActive(true);
            circle03.SetActive(true);
            circle04.SetActive(true);
            // Load the data in your ReadFile.cs script and pass it to the scene that needs it
            // Load the new scene
            StartCoroutine(LoadScene());
        }
        if (isMulti == true)
        {
            // Create an array of data to be sent with the event
            object[] eventData2 = new object[]
            {
            LoadBeatmapList.ActuallySongName,
            LoadBeatmapList.BPMValue,
            LoadBeatmapList.difficultymap,
            LoadBeatmapList.artistmap,
            LoadBeatmapList.imagePath,
            LoadBeatmapList.creatormap,

            LoadBeatmapList.IDSong
            };

            // Raise the update beatmap event
            PhotonNetworkingMessages.SendUpdateBeatmapEvent(eventData2);
            SceneManager.UnloadSceneAsync("MainLevelScene");
        }

    }

    public class PhotonNetworkingMessages
    {
        public const byte UpdateBeatmapEventCode = 10;

        public static void SendUpdateBeatmapEvent(object[] data)
        {
            PhotonNetwork.RaiseEvent(UpdateBeatmapEventCode, data, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }


    public void SaveSettings()
    {
        // Open the file for writing
        StreamWriter writer = new StreamWriter(pathSetting);

        // Write the modified settings to the file
        writer.WriteLine("Offset: " + Offset.text);
        writer.WriteLine("AR: " + AR.text);
        writer.WriteLine("BG Opacity: " + BG_Opacity.text);   
        // Close the file
        writer.Close();
        LoadSettings();
        SettingCanvas.SetActive(false);
    }

    private void LoadSettings()
    {
        if (File.Exists(pathSetting))
        {
            // Open the file for reading
            StreamReader reader = new StreamReader(pathSetting);

            // Read the settings from the file
            string offsetSetting = reader.ReadLine();
            string arSetting = reader.ReadLine();
            string bgOpacitySetting = reader.ReadLine();

            // Extract the values from the settings strings
            string offsetValueText = offsetSetting.Substring(offsetSetting.IndexOf(":") + 1).Trim();
            string arValueText = arSetting.Substring(arSetting.IndexOf(":") + 1).Trim();
            string bgOpacityValueText = bgOpacitySetting.Substring(bgOpacitySetting.IndexOf(":") + 1).Trim();

            UnityEngine.Debug.Log("offsetvalue la: " + offsetValueText);
            UnityEngine.Debug.Log("arValueText la: " + arValueText);
            UnityEngine.Debug.Log("bgOpacityValueText la: " + bgOpacityValueText);

            // Apply the values to the TextMeshPro text objects
            Offset.text = offsetValueText;
            AR.text = arValueText;
            BG_Opacity.text = bgOpacityValueText;

            OffsetValue=float.Parse(offsetValueText);
            ARValue=float.Parse(arValueText);
            BgOpacityValue=int.Parse(bgOpacityValueText);

            // Close the file
            reader.Close();
        }
        else
        {
            // Set default values if the file doesn't exist
            Offset.text = "0";
            AR.text = "6";
            BG_Opacity.text = "100";
        }
    }


    public string GetBPMValue(string bpmtext)
    {
        string bpmString = bpmtext.Replace("BPM: ", ""); // remove the "BPM: " prefix
        return bpmString;
    }


    public void OpenSettingCanvas()
    {
        SettingCanvas.SetActive(true);
    }

    Texture2D LoadTextureFromPath(string path, int maxWidth, int maxHeight)
    {
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // LoadImage() automatically resizes the texture to match the image size

            // Resize the texture if it's too large
            if (texture.width > maxWidth || texture.height > maxHeight)
            {
                float aspectRatio = (float)texture.width / (float)texture.height;
                int newWidth = Mathf.Min(texture.width, maxWidth);
                int newHeight = Mathf.Min(texture.height, maxHeight);

                if (texture.width > maxWidth)
                {
                    newHeight = Mathf.RoundToInt(newWidth / aspectRatio);
                }
                else if (texture.height > maxHeight)
                {
                    newWidth = Mathf.RoundToInt(newHeight * aspectRatio);
                }

                Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height);
                Color[] resizedPixels = new Color[newWidth * newHeight];

                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        float u = (float)x / (float)newWidth;
                        float v = (float)y / (float)newHeight;
                        resizedPixels[y * newWidth + x] = SampleTexture(pixels, texture.width, texture.height, u, v);
                    }
                }

                Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
                resizedTexture.SetPixels(resizedPixels);
                resizedTexture.Apply();
                UnityEngine.Object.Destroy(texture);
                return resizedTexture;
            }
        }
        else
        {
            Debug.LogWarning("File does not exist at path: " + path);
        }

        return texture;
    }

    Color SampleTexture(Color[] pixels, int width, int height, float u, float v)
    {
        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);
        //This ensures that the sample position is always within the texture boundaries.

        float x = u * (float)(width - 1);
        float y = v * (float)(height - 1);
        //The (width - 1) and (height - 1) terms are used to ensure that the sample position is always within the texture boundaries, even if u and v are exactly equal to 1.


        int x0 = Mathf.FloorToInt(x);
        int y0 = Mathf.FloorToInt(y);
        int x1 = Mathf.Clamp(x0 + 1, 0, width - 1);
        int y1 = Mathf.Clamp(y0 + 1, 0, height - 1);
        //x0 and y0 represent the top-left pixel coordinate of the four surrounding pixels, while x1 and y1 represent the bottom-right pixel coordinate. Clamp is used to ensure that the pixel coordinates are always within the texture boundaries.

        float sx = x - (float)x0;
        float sy = y - (float)y0;
        //These factors represent how far the sample position is between the top - left pixel and the top-right pixel(for sx) and between the top-left pixel and the bottom-left pixel(for sy).

        Color c00 = pixels[y0 * width + x0];
        Color c10 = pixels[y0 * width + x1];
        Color c01 = pixels[y1 * width + x0];
        Color c11 = pixels[y1 * width + x1];
        //c00, c10, c01, and c11 represent the colors of the top-left, top-right, bottom-left, and bottom-right pixels, respectively.

        Color top = Color.Lerp(c00, c10, sx); //top represents the color at the top of the sample position, interpolated between the top-left and top-right colors based on the sx factor.
        Color bottom = Color.Lerp(c01, c11, sx); //bottom represents the color at the bottom of the sample position, interpolated between the bottom-left and bottom-right colors based on the sx factor.

        return Color.Lerp(top, bottom, sy); //Interpolates between the top and bottom colors based on the sy factor, returning the final color at the sample position.
    }

    IEnumerator LoadScene()
    {


        panelfadeout.SetActive(true);       
        transitionAnim.SetBool("FadeOutOnly", true);
        circle1.SetBool("fadeintrue", true);
        circle2.SetBool("fadeintrue", true);
        circle3.SetBool("fadeintrue", true);
        circle4.SetBool("fadeintrue", true);
        Debug.Log("FadeOutOnly parameter value: " + transitionAnim.GetBool("FadeOutOnly"));
        //transitionAnim.SetTrigger("FadeOut");
        circle1.SetTrigger("EnlargeCircle");
        yield return new WaitForSeconds(0.5f);
        circle2.SetTrigger("EnlargeCircle");
        yield return new WaitForSeconds(0.5f);
        circle3.SetTrigger("EnlargeCircle");
        yield return new WaitForSeconds(0.5f);
        circle4.SetTrigger("EnlargeCircle");
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneName);
    }

    void ReturnNameSong (string SongNameWithID, out string Songname, out string ID)
    {
        // Find the index of the first space
        int firstSpaceIndex = SongNameWithID.IndexOf(' ');

        // Find the index of the last space
        int lastSpaceIndex = SongNameWithID.LastIndexOf(' ');

        // Extract the ID (remove leading/trailing whitespace if any)
        ID = SongNameWithID.Substring(0, firstSpaceIndex).Trim();

        // Extract the name (remove leading/trailing whitespace if any)
        Songname = SongNameWithID.Substring(lastSpaceIndex + 1).Trim();

    }

   

    IEnumerator FadeOutDisable()
    {
        
        yield return new WaitForSeconds(1.5f);
        panelfadeout.SetActive(false);
    }
}
