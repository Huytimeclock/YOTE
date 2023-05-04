using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class LoadBeatmapList : MonoBehaviour
{
    public GameObject parentObject;
    public RectTransform scrollViewContent;
    private string path = "";
    [SerializeField] TextMeshProUGUI Title_Song;
    [SerializeField] TextMeshProUGUI DiffText;
    [SerializeField] TextMeshProUGUI Artist;
    public Image imageTitle;

    void Start()
    {
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
                }
            }

            Title_Song.text = songName;
            Artist.text = artistName;
            DiffText.text=difficulty.ToString();
            // Instantiate the clone at the top of the scroll view content
            GameObject clone = Instantiate(parentObject, scrollViewContent);

        }
    }

    private void Update()
    {

    }



}
