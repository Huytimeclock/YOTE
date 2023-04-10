using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColorNormalInput : MonoBehaviour
{
    [SerializeField] string keytopress;
    public Image targetImage;
    public Color targetColor = Color.red;
    public float duration = 0f;
    public float returnDuration = 0.1f;

    private Color _startColor;
    private bool _isColorChanging = false;
    private bool _isColorReturning = false;
    private float _colorChangeTime = 0.0f;
    private float _returnTime = 0.0f;

    private void Start()
    {
        // Store the initial color of the image
        _startColor = targetImage.color;
    }

    private void Update()
    {
        // Check if the "H" key is pressed down
        if (Input.GetKeyDown(keytopress))
        {
            // Start the color change if it is not already in progress
            if (!_isColorChanging && !_isColorReturning)
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
                _isColorReturning = true;
                _returnTime = 0.0f;
            }
        }

        // Check if the color return is in progress
        if (_isColorReturning)
        {
            // Calculate the normalized time for the color return
            float t = Mathf.Clamp01(_returnTime / returnDuration);

            // Calculate the new color for the image
            Color newColor = Color.Lerp(targetColor, _startColor, t);

            // Set the new color for the image
            targetImage.color = newColor;

            // Update the color return time
            _returnTime += Time.deltaTime;

            // Check if the color return is complete
            if (_returnTime >= returnDuration)
            {
                _isColorReturning = false;
            }
        }
    }
}
