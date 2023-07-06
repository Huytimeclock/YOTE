using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPlayTab : MonoBehaviour
{
    public GameObject bar_play;
    public GameObject Logo_play;
    public GameObject Single_play;
    public GameObject Multi_play;
    public GameObject Back;

    public GameObject bar_option;
    public GameObject option_logo;
    public GameObject bar_edit;
    public GameObject edit_logo;
    public GameObject bar_exit;
    public GameObject exit_logo;

    public static int isClickedTab = 0;





    // Start is called before the first frame update
    void Start()
    {
        Single_play.SetActive(false);
        Back.SetActive(false);
        Multi_play.SetActive(false);



        Single_play.transform.position += new Vector3 (0f, 10f, 0f);
        Back.transform.position += new Vector3(10f, 0, 0);
        Multi_play.transform.position += new Vector3(0, -10f, 0);
        
    }

    public void EnterPlayTab1()
    {
        if (isClickedTab == 0)
        {
            bar_option.SetActive(false);
            option_logo.SetActive(false);
            bar_edit.SetActive(false);
            edit_logo.SetActive(false);
            bar_exit.SetActive(false);
            exit_logo.SetActive(false);
            StartCoroutine(OpenPlayTab(0.2f));
            isClickedTab = 1;
        }
    }
    public IEnumerator OpenPlayTab(float time)
    {
        float elapsedTime = 0;
        Vector3 button1StartPos = bar_play.transform.position;
        Vector3 button2StartPos = Logo_play.transform.position;
        Vector3 singlebut = Single_play.transform.position;
        Vector3 backbut = Back.transform.position;
        Vector3 multibut = Multi_play.transform.position;

        Single_play.SetActive(true);
        Back.SetActive(true);
        Multi_play.SetActive(true);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / time);

            Vector3 button1TargetPos = button1StartPos + new Vector3(0, -1, 0);
            Vector3 button2TargetPos = button2StartPos + new Vector3(0, -1, 0);
            Vector3 singlebutTar =singlebut+ new Vector3(0, -10f, 0);
            Vector3 backbutTar =backbut+ new Vector3(-10f, 0, 0);
            Vector3 multibutTar =multibut+ new Vector3(0, 10f, 0);

            bar_play.transform.position = Vector3.Lerp(button1StartPos, button1TargetPos, t);
            Logo_play.transform.position = Vector3.Lerp(button2StartPos, button2TargetPos, t);

            Single_play.transform.position = Vector3.Lerp(singlebut, singlebutTar, t);
            Back.transform.position = Vector3.Lerp(backbut, backbutTar, t);
            Multi_play.transform.position = Vector3.Lerp(multibut, multibutTar, t);

            yield return null;
        }
    }
    public void BackTab()
    {
        Single_play.SetActive(false);
        Back.SetActive(false);
        Multi_play.SetActive(false);
        bar_option.SetActive(true);
        option_logo.SetActive(true);
        bar_edit.SetActive(true);
        edit_logo.SetActive(true);
        bar_exit.SetActive(true);
        exit_logo.SetActive(true);
        isClickedTab = 0;
        bar_play.transform.position += new Vector3(0f, 1f, 0f);
        Logo_play.transform.position += new Vector3(0f, 1f, 0f);
        Single_play.transform.position += new Vector3(0f, 10f, 0f);
        Back.transform.position += new Vector3(10f, 0, 0);
        Multi_play.transform.position += new Vector3(0, -10f, 0);
    }
    public void LoadSingleScene()
    {
        isClickedTab = 0;
        SceneManager.LoadScene("MainLevelScene");
    }
}
