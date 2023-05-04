using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadBeatmapList : MonoBehaviour
{
    public GameObject parentObject;
    public RectTransform scrollViewContent;
    private string path = "";


    void Start()
    {
        path = Application.dataPath + "\\Game_data\\Beatmaps";
        string[] folderPaths = Directory.GetDirectories(path);

        foreach (string folderPath in folderPaths)
        {
            // Instantiate the clone at the top of the scroll view content
            GameObject clone = Instantiate(parentObject, scrollViewContent);

            // Set any necessary properties on the clone object here

            // If the clone object has a Text component, set its text to the folder name
            Text cloneText = clone.GetComponentInChildren<Text>();
            if (cloneText != null)
            {
                cloneText.text = Path.GetFileName(folderPath);
            }
        }
    }

    private void Update()
    {

    }



}
