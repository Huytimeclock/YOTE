using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColorNormalInput : MonoBehaviour
{
    [SerializeField] string keytopress;
    public Image targetImage;
    public Color targetColor = Color.red;
    public Color targetAirColor = Color.green;

    bool isShiftPressed = false;



    private Color _startColor;


    private void Start()
    {
        // Store the initial color of the image
        _startColor = targetImage.color;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isShiftPressed = true;
        }

        // Check if the "H" key is pressed down
        if (Input.GetKey(keytopress) && isShiftPressed == false)
        {
            // Calculate the new color for the image
            Color newColor = targetColor;

            // Set the new color for the image
            targetImage.color = newColor;
        }
        else if (Input.GetKey(keytopress) && isShiftPressed)
        {

            Color newColor = targetAirColor;
            Debug.Log("activated!");
            // Set the new color for the image
            targetImage.color = newColor;

        }

        else
        {
            targetImage.color = _startColor;
            isShiftPressed = false;
        }



    }
}
