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
    [SerializeField] TextMeshProUGUI Title_Song;


    [SerializeField] GameObject[] Rank;  

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
        SceneManager.UnloadSceneAsync("Gameplay");
    }






}
