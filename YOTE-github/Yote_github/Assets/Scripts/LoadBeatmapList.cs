using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using System.Collections;


public class LoadBeatmapList : MonoBehaviour
{
    public GameObject parentObject;
    public RectTransform scrollViewContent;
    private string path = "";
    [SerializeField] TextMeshProUGUI Title_Song;
    [SerializeField] TextMeshProUGUI DiffText;
    [SerializeField] TextMeshProUGUI Artist;
    [SerializeField] TextMeshProUGUI BPM1;
    [SerializeField] TextMeshProUGUI CreatorText;
    [SerializeField] AudioSource EntrySound;
    public Image imageTitle;
    public Image imageComponent;
    private GameObject parentContainer;
    private string imagePath = "";
    private string infoPath = "";
    private string songPath = "";
    private string BPMValue = "";


    public Animator transitionAnim;
    public string SceneName;
    [SerializeField] GameObject panelfadeout;


    void Start()
    {
        StartCoroutine(FadeOutDisable());
        path = Application.dataPath + "\\Game_data\\Beatmaps";
        string[] folderPaths = Directory.GetDirectories(path);

        foreach (string folderPath in folderPaths)
        {
           

            // Set any necessary properties on the clone object here

            // Get the name of the song (which is the folder name)
            string songName = Path.GetFileName(folderPath);

            // Get the path to the image file
            string imagePath = Path.Combine(folderPath, "bg.jpg");

            // Get the path to the info file
            string infoPath = Path.Combine(folderPath, "map.txt");

            // Parse the info file to get the artist name and difficulty
            string artistName = "";
            int difficulty = -1;
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

            Title_Song.text = songName;
            Artist.text = artistName;
            DiffText.text=difficulty.ToString();
            BPM1.text = "BPM: " + BPM.ToString();
            CreatorText.text = Creator;

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
        string songName = parentObject.transform.Find("Song_title").GetComponent<TextMeshProUGUI>().text;
        BPMValue = parentObject.transform.Find("BPM").GetComponent<TextMeshProUGUI>().text;
        Debug.Log("songName " + songName);
        // Get the path to the selected song data
        string folderPath = Path.Combine(path, songName);
        imagePath = Path.Combine(folderPath, "bg.jpg");
        infoPath = Path.Combine(folderPath, "map.txt");
        songPath = Path.Combine(folderPath, "audio.mp3");



        // Load the data in your ReadFile.cs script and pass it to the scene that needs it

        // Load the new scene
        StartCoroutine(LoadScene());

    }


    public string getImagePath1()
    {
        return imagePath;
    }

    public string getMapPath1()
    {
        return infoPath;
    }

    public string GetSongPath1()
    {
        return songPath;
    }

    public string GetBPMValue()
    {
        string bpmString = BPMValue.Replace("BPM: ", ""); // remove the "BPM: " prefix
        return bpmString;
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
        transitionAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }


    IEnumerator FadeOutDisable()
    {
        
        yield return new WaitForSeconds(1.5f);
        panelfadeout.SetActive(false);
    }
}
