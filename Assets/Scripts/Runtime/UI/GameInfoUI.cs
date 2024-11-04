using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameInfoUI : MonoBehaviour, IPanel
{
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private CanvasGroup canvasGroup;

    public void UpdateMoves(int moves)
    {
        if (movesText == null) return;

        movesText.text = moves.ToString();
        if (moves <= 5)
        {
            movesText.color = Color.red;
            movesText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 1, 0.5f);
        }
        else
        {
            movesText.color = Color.white;
        }
    }

    public void UpdateTime(float timeLeft)
    {
        if (timeText == null) return;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";

        if (timeLeft <= 10)
        {
            timeText.color = Color.Lerp(Color.red, Color.white, timeLeft % 1);
        }
        else
        {
            timeText.color = Color.white;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.5f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
