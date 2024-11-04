using System;
using Match2.Core.Data;
using Match2.Core.Services.LevelLogic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameTimer gameTimer;
    private MoveCounter moveCounter;
    private GameState currentState;
    private LevelData levelData;

    public event Action<float> OnTimeChanged;
    public event Action<int> OnMovesChanged;
    public event Action<GameState> OnGameStateChanged;
    public event Action OnGameStarted;

    private void Awake()
    {
        gameTimer = new GameTimer();
        moveCounter = new MoveCounter();
        currentState = GameState.None;
    }

    private void Update()
    {
        if (currentState != GameState.Playing) return;

        UpdateTimer();
        CheckGameEndConditions();
    }

    public void Initialize(LevelData levelData)
    {
        this.levelData = levelData;
    }

    public void StartGame()
    {
        InitializeGameState();
        SetState(GameState.Playing);
        gameTimer.StartTimer();
        OnGameStarted?.Invoke();
    }

    private void InitializeGameState()
    {
        gameTimer.Initialize(levelData.timeLimit);
        moveCounter.Initialize(levelData.totalMoves);

        NotifyUIUpdates();
    }

    private void NotifyUIUpdates()
    {
        OnTimeChanged?.Invoke(gameTimer.CurrentTime);
        OnMovesChanged?.Invoke(moveCounter.RemainingMoves);
    }

    public void OnMovePerformed()
    {
        if (currentState != GameState.Playing) return;

        moveCounter.DecreaseMoves();
        OnMovesChanged?.Invoke(moveCounter.RemainingMoves);

        CheckGameEndConditions();
    }

    private void UpdateTimer()
    {
        gameTimer.Update(Time.deltaTime);
        OnTimeChanged?.Invoke(gameTimer.CurrentTime);
    }

    private void CheckGameEndConditions()
    {
        if (!moveCounter.IsMovesLeft() || gameTimer.IsTimeUp())
        {
            EndGame(GameState.Lost);
        }
    }

    private void EndGame(GameState endState)
    {
        gameTimer.StopTimer();
        SetState(endState);
    }

    public GameState GetCurrentState() => currentState;

    private void SetState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnGameStateChanged?.Invoke(currentState);
    }

    public void OnLevelCompleted()
    {
        if (currentState != GameState.Playing) return;
        EndGame(GameState.Won);
    }
}
public enum GameState
{
    None,
    Playing,
    Won,
    Lost,
}
