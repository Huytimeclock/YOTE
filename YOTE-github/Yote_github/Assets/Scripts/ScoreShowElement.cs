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
    [SerializeField] TextMeshProUGUI DiffText;
    [SerializeField] TextMeshProUGUI Artist;

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
        DiffText.text = readFile.GetDiff().ToString();
        Artist.text = readFile.GetArtistText().ToString();

        Texture2D textureSmallImage = LoadTextureFromPath(imagePath, 400, 400);
        Texture2D textureLargeImage = LoadTextureFromPath(imagePath, 1920, 1080);

        if (textureSmallImage != null) // image for song-bg (400x400)
        {
            Sprite sprite = Sprite.Create(textureSmallImage, new Rect(0, 0, textureSmallImage.width, textureSmallImage.height), Vector2.zero);
            imageTitle.sprite = sprite;
        }

        if (textureLargeImage != null) // image for big-bg (1980x1020)
        {
            Sprite sprite = Sprite.Create(textureLargeImage, new Rect(0, 0, textureLargeImage.width, textureLargeImage.height), Vector2.zero);
            imageComponent.sprite = sprite;
            Color imageColor = imageComponent.color;
            imageColor.a = 0.2f; // Set alpha value to 20%
            imageComponent.color = imageColor;
        }


        SceneManager.UnloadSceneAsync("Gameplay");
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






}
