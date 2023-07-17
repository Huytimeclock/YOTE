using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Mono.Cecil;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics.Tracing;
using static LoadBeatmapList;

public class UpdateBeatap : MonoBehaviourPunCallbacks
{


    [SerializeField] TextMeshProUGUI Title_Song;
    [SerializeField] TextMeshProUGUI DiffText;
    [SerializeField] TextMeshProUGUI Artist;
    [SerializeField] TextMeshProUGUI BPM1;
    [SerializeField] TextMeshProUGUI CreatorText;
    [SerializeField] TextMeshProUGUI SongPathName;
    public Image imageTitle;
    Texture2D textureSmallImage;


    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon network.");
            return;
        }

        PhotonNetwork.NetworkingClient.EventReceived += OnCustomEvent;
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnCustomEvent;
    }



    public void OnSwitChSceneClick()
    {
        LoadBeatmapList.isMulti = true;



        SceneManager.LoadScene("MainLevelScene", LoadSceneMode.Additive);
    }


    public void OnLoadGame()
    {
        ReadFile.isMulti = true;
        // Raise a custom network event to notify all players to switch scenes
        PhotonNetwork.RaiseEvent(20, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }


    public void OnUpdateBeatmap(object[] data)
    {
        Title_Song.text = LoadBeatmapList.ActuallySongName;
        Artist.text = LoadBeatmapList.artistmap;
        DiffText.text = LoadBeatmapList.difficultymap.ToString();
        BPM1.text = LoadBeatmapList.BPMValue;
        CreatorText.text = LoadBeatmapList.creatormap;
        textureSmallImage = LoadTextureFromPath(LoadBeatmapList.imagePath, 400, 400);

        if (textureSmallImage != null)
        {
            Sprite sprite = Sprite.Create(textureSmallImage, new Rect(0, 0, textureSmallImage.width, textureSmallImage.height), Vector2.zero);
            imageTitle.sprite = sprite;
        }
    }

    private void OnCustomEvent(EventData eventData)
    {
        if (eventData.Code == PhotonNetworkingMessages.UpdateBeatmapEventCode)
        {
            object[] data = (object[])eventData.CustomData;

            // Update the static variables with the received data
            LoadBeatmapList.ActuallySongName = (string)data[0];
            LoadBeatmapList.BPMValue = (string)data[1];
            LoadBeatmapList.difficultymap = (int)data[2];
            LoadBeatmapList.artistmap = (string)data[3];
            LoadBeatmapList.imagePath = (string)data[4];
            LoadBeatmapList.creatormap = (string)data[5];

            LoadBeatmapList.IDSong= (string)data[6];

            // Call the method to update the UI or perform any other necessary actions
            OnUpdateBeatmap(data);
        }
        if (eventData.Code == 20)
        {
            ReadFile.isMulti = true;
            // All players received the custom network event, switch scenes
            PhotonNetwork.LoadLevel("Gameplay");
        }
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
