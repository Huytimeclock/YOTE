using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImageColorShiftInput : MonoBehaviour
{
    public SpriteRenderer RightShift;
    public SpriteRenderer LeftShift;
    public Color defaultColor = Color.white;
    public Color shiftColor = Color.green;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) )
        {
            // Calculate the new color for the image
            Color newColor = shiftColor;

            // Set the new color for the image
            LeftShift.color = newColor;
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            // Calculate the new color for the image
            Color newColor = shiftColor;

            // Set the new color for the image
            RightShift.color = newColor;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            // Perform actions when right shift button is released
            Color newColor = defaultColor;
            RightShift.color = newColor;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Perform actions when right shift button is released
            Color newColor = defaultColor;
            LeftShift.color = newColor;
        }
    }
}
