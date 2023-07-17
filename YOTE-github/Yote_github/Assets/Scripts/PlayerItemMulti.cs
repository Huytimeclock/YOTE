using UnityEngine;
using TMPro;

public class PlayerItemMulti : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text percentageText;

    public void Initialize(string playerName, float percentage)
    {
        playerNameText.text = playerName;
        percentageText.text = percentage.ToString();
    }

    public void UpdatePercentage(float newPercentage)
    {
        percentageText.text = newPercentage.ToString();
    }
}
