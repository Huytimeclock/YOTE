using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInScene : MonoBehaviour
{
    public float fadeTime = 1.0f;
    public Image fadePanel;

    private void Start()
    {
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        // Set the initial color of the panel
        Color color = fadePanel.color;
        color.a = 1f;
        fadePanel.color = color;

        // Fade in the panel
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeTime);
            color.a = Mathf.Lerp(1f, 0f, normalizedTime);
            fadePanel.color = color;
            yield return null;
        }
    }
}
