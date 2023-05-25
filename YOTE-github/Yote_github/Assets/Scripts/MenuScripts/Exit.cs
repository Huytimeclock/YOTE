using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject ExitPanel;
    bool isExit = false;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        while (isExit == false)
        {

            StartCoroutine(ExitOverTime(0.2f));
            isExit = true;
        }
    }

    public IEnumerator ExitOverTime(float time)
    {
        float elapsedTime = 0;
        Vector3 PosExitPanelStart = ExitPanel.transform.position;
        Vector3 PosExitPanelEnd = ExitPanel.transform.position + new Vector3(0, -3.05f, 0);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / time);


            ExitPanel.transform.position = Vector3.Lerp(PosExitPanelStart, PosExitPanelEnd, t);


            yield return null;
        }
    }

    public void StartBackExit ()
    {
        Debug.Log("clicked");
        isExit = false;
        StartCoroutine (NotExitOverTime(0.2f));
    }
    public IEnumerator NotExitOverTime(float time)
    {
        float elapsedTime = 0;
        Vector3 PosExitPanelStart = ExitPanel.transform.position;
        Vector3 PosExitPanelEnd = ExitPanel.transform.position + new Vector3(0, 3.05f, 0);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / time);


            ExitPanel.transform.position = Vector3.Lerp(PosExitPanelStart, PosExitPanelEnd, t);


            yield return null;
        }
    }


    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
