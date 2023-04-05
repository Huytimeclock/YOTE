using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverLogo : MonoBehaviour
{
    [SerializeField] GameObject logo1;
    [SerializeField] GameObject logo2;
    [SerializeField] GameObject logo3;
    [SerializeField] GameObject logo4;

    private void OnMouseEnter()
    {
        logo1.transform.localScale = new Vector3(55f, 55f, 55f);
        logo2.transform.localScale = new Vector3(55f, 55f, 55f);
        logo3.transform.localScale = new Vector3(55f, 55f, 55f);
        logo4.transform.localScale = new Vector3(55f, 55f, 55f);
    }

    private void OnMouseExit()
    {
        logo1.transform.localScale = new Vector3(50f, 50f, 50f);
        logo2.transform.localScale = new Vector3(50f, 50f, 50f);
        logo3.transform.localScale = new Vector3(50f, 50f, 50f);
        logo4.transform.localScale = new Vector3(50f, 50f, 50f);
    }
    
}
