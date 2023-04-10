using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColor : MonoBehaviour
{
    public Image targetImage;
    public Color targetColor = Color.red;
    public float duration = 1.0f;

    private Color _startColor;
    private bool _isColorChanging = false;
    private float _colorChangeTime = 0.0f;

    private void Start()
    {
        // Store the initial color of the image
        _startColor = targetImage.color;
    }

    private void Update()
    {
        // Check if the "H" key is pressed down
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Start the color change if it is not already in progress
            if (!_isColorChanging)
            {
                _isColorChanging = true;
                _colorChangeTime = 0.0f;
            }
        }

        // Check if the color change is in progress
        if (_isColorChanging)
        {
            // Calculate the normalized time for the color change
            float t = Mathf.Clamp01(_colorChangeTime / duration);

            // Calculate the new color for the image
            Color newColor = Color.Lerp(_startColor, targetColor, t);

            // Set the new color for the image
            targetImage.color = newColor;

            // Update the color change time
            _colorChangeTime += Time.deltaTime;

            // Check if the color change is complete
            if (_colorChangeTime >= duration)
            {
                _isColorChanging = false;
            }
        }
    }
}
