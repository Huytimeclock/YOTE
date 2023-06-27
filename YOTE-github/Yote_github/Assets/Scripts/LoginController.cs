using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public GameObject LoginSection;
    public GameObject RegisterSection;
    public GameObject ErrorMessage;
    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        LoginSection.SetActive(false);
        RegisterSection.SetActive(false);
        ErrorMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back_to_login_from_regis()
    {
        LoginSection.SetActive(true);
        RegisterSection.SetActive(false);
    }

    public void Go_to_regis_from_login()
    {
        LoginSection.SetActive(false);
        RegisterSection.SetActive(true);
    }

    public void AppearOrDisappearLoginSection()
    {
        if (isActive)
        {
            LoginSection.SetActive(false);
            RegisterSection.SetActive(false);
            isActive = false;
            
        }
        else
        {
            LoginSection.SetActive(true);
            isActive = true;
            
        }
    }

}
