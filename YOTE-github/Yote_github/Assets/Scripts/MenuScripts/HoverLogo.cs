using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverLogo : MonoBehaviour
{
    [SerializeField] GameObject logo1;
    [SerializeField] GameObject logo2;
    [SerializeField] GameObject logo3;
    [SerializeField] GameObject logo4;

    private void OnMouseOver()
    {
        logo1.transform.localScale = new Vector3(5f, 5f, 5f);
    }

    private void OnMouseExit()
    {
        logo1.transform.localScale = new Vector3(3f, 3f, 3f);
    }
}
