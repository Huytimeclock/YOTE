using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInScene : MonoBehaviour
{
    public float fadeTime = 1.0f;
    public Image fadePanel;
    public Image bg;

    private void Start()
    {
        StartCoroutine(FadeInPanelAndBg());
    }

    IEnumerator FadeInPanelAndBg()
    {
        // Set the initial color of the panel and the bg image
        Color panelColor = fadePanel.color;
        panelColor.a = 1f;
        fadePanel.color = panelColor;

        Color bgColor = bg.color;
        bgColor.a = 0f;
        bg.color = bgColor;

        // Fade in the panel and the bg image
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeTime);
            panelColor.a = Mathf.Lerp(1f, 0f, normalizedTime);
            fadePanel.color = panelColor;

            bgColor.a = Mathf.Lerp(0f, 0.2f, normalizedTime);
            bg.color = bgColor;

            yield return null;
        }

    }
}
