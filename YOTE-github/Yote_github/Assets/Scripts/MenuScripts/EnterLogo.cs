using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLogo : MonoBehaviour
{
    [SerializeField] GameObject Button1;
    [SerializeField] GameObject Button2;
    [SerializeField] GameObject Button3;
    [SerializeField] GameObject Button4;
    // Start is called before the first frame update
    void Start()
    {
        Button1.SetActive(false); 
        Button2.SetActive(false);
        Button3.SetActive(false);
        Button4.SetActive(false);
    }

    private void OnMouseDown()
    {
        Button1.SetActive(true);
        Button2.SetActive(true);
        Button3.SetActive(true);
        Button4.SetActive(true);
    }
}
