using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreShowElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CPCount;
    [SerializeField] TextMeshProUGUI PerfectCount;
    [SerializeField] TextMeshProUGUI GreatCount;
    [SerializeField] TextMeshProUGUI GoodCount;
    [SerializeField] TextMeshProUGUI MissCount;
    [SerializeField] TextMeshProUGUI PercentageValue;

    private void Start()
    {
        // Find the ReadFile game object in Scene 1
        GameObject readFileObj = GameObject.Find("ReadFile");

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

        // Set the text of the TextMeshProUGUI components to the values from ReadFile
        CPCount.text = readFile.GetCPCount().ToString();
        PerfectCount.text =  readFile.GetPerfectCount().ToString();
        GreatCount.text =  readFile.GetGreatCount().ToString();
        GoodCount.text =  readFile.GetGoodCount().ToString();
        MissCount.text =  readFile.GetMissCount().ToString();
        PercentageValue.text = readFile.GetPercentage().ToString("F2");
        SceneManager.UnloadSceneAsync("Gameplay");
    }






}
