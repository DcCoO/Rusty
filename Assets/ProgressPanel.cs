using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : SingletonMonoBehaviour<ProgressPanel>
{
    [SerializeField] Image innerBar;
    [SerializeField] TMP_Text progressText;

    public void SetPercentage(float percentage = 0)
    {
        innerBar.fillAmount = percentage;
        progressText.text = $"{(int)Mathf.Round(100f * percentage)}%";
    }
}
