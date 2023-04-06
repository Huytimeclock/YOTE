using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Button1.transform.Translate(-3.4f, 0, 0);
            Button2.transform.Translate(3.4f, 0, 0);
            Button3.transform.Translate(-3.4f, 0, 0);
            Button4.transform.Translate(3.4f, 0, 0);
            onenter = true;
        }
       
    }
}
