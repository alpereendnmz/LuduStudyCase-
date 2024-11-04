using Match2.Core;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private GameInfoUI gameInfoUI;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private WinUI winUI;

    [Header("Game Systems")]
    [SerializeField] private GameController gameController;
    private GameStateManager gameStateManager;

    public void Initialize(GameStateManager gameStateManager)
    {
        this.gameStateManager = gameStateManager;

        InitializeUIPanels();
        SubscribeToEvents();
        ShowMenu();
    }

    private void InitializeUIPanels()
    {
        menuUI.Initialize(this);
        gameOverUI.Initialize(OnRetryClicked, OnMenuClicked);
        winUI.Initialize(OnRetryClicked, OnMenuClicked);
    }

    private void SubscribeToEvents()
    {
        gameStateManager.OnMovesChanged += UpdateMoves;
        gameStateManager.OnTimeChanged += UpdateTime;
        gameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                ShowGameInfo();
                break;
            case GameState.Lost:
                ShowGameOver();
                break;
            case GameState.Won:
                ShowWin();
                break;
        }
    }
    private void UpdateMoves(int moves)
    {
        gameInfoUI.UpdateMoves(moves);
    }

    private void UpdateTime(float timeLeft)
    {
        gameInfoUI.UpdateTime(timeLeft);
    }

    public void ShowMenu()
    {
        HideAllPanels();
        menuUI.Show();
    }

    public void ShowGame()
    {
        HideAllPanels();

        gameInfoUI.Show();
        gameController.StartGame();
    }

    private void ShowGameInfo()
    {
        HideAllPanels();

        gameInfoUI.Show();
    }

    private void ShowGameOver()
    {
        HideAllPanels();
        gameOverUI.Show();
    }

    private void ShowWin()
    {
        HideAllPanels();
        winUI.Show();
    }

    private void HideAllPanels()
    {
        menuUI.Hide();
        gameInfoUI.Hide();
        gameOverUI.Hide();
        winUI.Hide();
    }

    private void OnRetryClicked()
    {
        ShowGame();
    }

    private void OnMenuClicked()
    {
        ShowMenu();
    }

    private void OnDestroy()
    {
        if (gameStateManager != null)
        {
            gameStateManager.OnMovesChanged -= UpdateMoves;
            gameStateManager.OnTimeChanged -= UpdateTime;
            gameStateManager.OnGameStateChanged -= HandleGameStateChanged;
        }
    }
}