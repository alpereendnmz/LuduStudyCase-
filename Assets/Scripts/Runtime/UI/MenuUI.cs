using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour, IPanel
{
    [SerializeField] private Button playButton;

    private GameUIController gameUIController;

    public void Initialize(GameUIController gameUIController)
    {
        this.gameUIController = gameUIController;
        SetupButton();
    }

    private void SetupButton()
    {
        playButton.onClick.AddListener(() =>
        {
            gameUIController.ShowGame();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
        }
    }
}