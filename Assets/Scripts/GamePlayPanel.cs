using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public Image countDownImage;
    public GameObject winPanel;
    public GameObject losePanel;

    public void SetCountDown(float remainingTime, float maxTime)
    {
        if (countDownText != null)
        {
            int displayTime = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));
            bool showText = displayTime > 0;
            countDownText.gameObject.SetActive(showText);
            if (showText)
            {
                countDownText.text = displayTime.ToString();
            }
        }

        if (countDownImage != null)
        {
            float normalized = (maxTime > 0f) ? Mathf.Clamp01(remainingTime / maxTime) : 0f;
            countDownImage.fillAmount = normalized;
        }
    }

    private void OnEnable()
    {
        if (countDownText != null)
        {
            countDownText.gameObject.SetActive(false);
        }

        if (countDownImage != null)
        {
            countDownImage.fillAmount = 0f;
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    public void ShowWinPanel(bool isShow, int star, int reward){
        AudioManager.Instance.PlayWinSound();
        winPanel.SetActive(isShow);
        winPanel.GetComponent<WinPanel>().Init(star, reward);
    }

    public void ShowLosePanel(bool isShow){
        AudioManager.Instance.PlayLoseSound();
        losePanel.SetActive(isShow);
    }
}
