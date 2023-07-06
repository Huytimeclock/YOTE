using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLogo : MonoBehaviour
{
    [SerializeField] GameObject Button1;
    [SerializeField] GameObject Button2;
    [SerializeField] GameObject Button3;
    [SerializeField] GameObject Button4;
    bool onenter = false;
    // Start is called before the first frame update
    void Start()
    {
        Button1.SetActive(false); 
        Button2.SetActive(false);
        Button3.SetActive(false);
        Button4.SetActive(false);
    }
    public bool ReturnOnEnter() // use to check if the bar is already collapsed so u can call in EnterPlaySesson ( will use later )
    {
        return onenter;
    }
    private void OnMouseDown()
    {
        while (onenter==false)  // move bar
        {
            Button1.SetActive(true);
            Button2.SetActive(true);
            Button3.SetActive(true);
            Button4.SetActive(true);
            StartCoroutine(MoveButtonsOverTime(0.2f));
            onenter = true;
        }
       
    }



    public IEnumerator MoveButtonsOverTime(float time)
    {
        float elapsedTime = 0;
        Vector3 button1StartPos = Button1.transform.position;
        Vector3 button2StartPos = Button2.transform.position;
        Vector3 button3StartPos = Button3.transform.position;
        Vector3 button4StartPos = Button4.transform.position;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / time);

            Vector3 button1TargetPos = button1StartPos + new Vector3(-3.4f, 0, 0);
            Vector3 button2TargetPos = button2StartPos + new Vector3(3.4f, 0, 0);
            Vector3 button3TargetPos = button3StartPos + new Vector3(-3.4f, 0, 0);
            Vector3 button4TargetPos = button4StartPos + new Vector3(3.4f, 0, 0);

            Button1.transform.position = Vector3.Lerp(button1StartPos, button1TargetPos, t);
            Button2.transform.position = Vector3.Lerp(button2StartPos, button2TargetPos, t);
            Button3.transform.position = Vector3.Lerp(button3StartPos, button3TargetPos, t);
            Button4.transform.position = Vector3.Lerp(button4StartPos, button4TargetPos, t);

            yield return null;
        }
    }



}
