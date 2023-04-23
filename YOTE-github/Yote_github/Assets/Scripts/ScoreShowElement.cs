using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class ScoreShowElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CPCount;
    [SerializeField] TextMeshProUGUI PerfectCount;
    [SerializeField] TextMeshProUGUI GreatCount;
    [SerializeField] TextMeshProUGUI GoodCount;
    [SerializeField] TextMeshProUGUI MissCount;
    [SerializeField] TextMeshProUGUI PercentageValue;
    [SerializeField] TextMeshProUGUI Title_Song;


    [SerializeField] GameObject[] Rank;

    private string imagePath;
    public Image imageComponent;
    public Image imageTitle;

    private void Start()
    {
        

        // Find the ReadFile game object in Scene 1
        GameObject readFileObj = GameObject.Find("ReadFile");

        for (int i = 0; i < Rank.Length; i++)
        {
            Rank[i].SetActive(false);
        }
        if (readFileObj == null)
        {
            Debug.LogError("ReadFile object not found");
            return;
        }

        // Get the ReadFile script from the game object
        ReadFile readFile = readFileObj.GetComponent<ReadFile>();

        if (readFile == null)
        {
            Debug.LogError("ReadFile script not found");
            return;
        }

        float percentage = readFile.GetPercentage();

        if(percentage <80)
        {
            Rank[6].SetActive(true);
        }
        else if(percentage<97)
        {
            Rank[5].SetActive(true);
        }
        else if (percentage<98)
        {
            Rank[4].SetActive(true);
        }
        else if (percentage<99)
        {
            Rank[3].SetActive(true);
        }
        else if (percentage<99.5)
        {
            Rank[2].SetActive(true);
        }
        else if (percentage<99.4)
        {
            Rank[1].SetActive(true);
        }
        else if (percentage ==100)
        {
            Rank[0].SetActive(true);
        }

        // Set the text of the TextMeshProUGUI components to the values from ReadFile
        CPCount.text = readFile.GetCPCount().ToString();
        PerfectCount.text =  readFile.GetPerfectCount().ToString();
        GreatCount.text =  readFile.GetGreatCount().ToString();
        GoodCount.text =  readFile.GetGoodCount().ToString();
        MissCount.text =  readFile.GetMissCount().ToString();
        PercentageValue.text = readFile.GetPercentage().ToString("F2");
        Title_Song.text=readFile.GetMapName().ToString();
        imagePath = readFile.GetImagePath().ToString();

        Texture2D texture = LoadTextureFromPath(imagePath);

        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            imageComponent.sprite = sprite;
            imageTitle.sprite = sprite;
            Color imageColor = imageComponent.color;
            imageColor.a = 0.2f; // Set alpha value to 20%
            imageComponent.color = imageColor;
        }


        SceneManager.UnloadSceneAsync("Gameplay");
    }
    Texture2D LoadTextureFromPath(string path)
    {
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // LoadImage() automatically resizes the texture to match the image size
        }
        else
        {
            Debug.LogWarning("File does not exist at path: " + path);
        }

        return texture;
    }





}
