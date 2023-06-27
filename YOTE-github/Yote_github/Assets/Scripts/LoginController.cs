using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public GameObject LoginSection;
    public GameObject RegisterSection;

    // Start is called before the first frame update
    void Start()
    {
        LoginSection.SetActive(false);
        RegisterSection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back_to_login_from_regis()
    {
        LoginSection.SetActive(false);
        RegisterSection.SetActive(true);
    }
}
