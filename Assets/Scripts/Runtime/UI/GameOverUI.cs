using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour, IPanel
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    public void Initialize(UnityAction onRetry, UnityAction onMenu)
    {
        retryButton.onClick.AddListener(onRetry);
        menuButton.onClick.AddListener(onMenu);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
    }
}
