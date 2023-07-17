using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       string pathSetting = Application.dataPath + "\\Game_data\\settings.txt";
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

            LoadBeatmapList.OffsetValue=float.Parse(offsetValueText);           
            LoadBeatmapList.ARValue = float.Parse(arValueText);
            LoadBeatmapList.BgOpacityValue = int.Parse(bgOpacityValueText);

            // Close the file
            reader.Close();
        }
        else
        {
            // Set default values if the file doesn't exist
            // set text for error not founding config file
        }
    }


}
