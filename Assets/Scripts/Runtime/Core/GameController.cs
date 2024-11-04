using Match2.Core.Cube;
using Match2.Core.Data;
using Match2.Core.Grid;
using Match2.Core.Services.Animation;
using Match2.Core.Services.Gravity;
using Match2.Core.Services.Matching;
using Match2.Core.Util;
using UnityEngine;

namespace Match2.Core
{
    public class GameController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelData levelData;
        [SerializeField] private CubeDatabase cubeDatabase;

        [Header("Game Systems")]
        [SerializeField] private GridController gridController;
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private GameUIController gameUIController;

        [Header("Cube Pool")]
        [SerializeField] private CubePool cubePool;

        private CubeFactory cubeFactory;
        private MatchFinder matchFinder;
        private AnimationController animationController;
        private PowerupManager powerupManager;
        private GravityProcessor gravityProcessor;
        private MatchProcessor matchProcessor;

        private int currentMatchCount = 0;
        private bool isInitialized = false;

        private void Awake()
        {
            Application.targetFrameRate = 120;

            gameUIController.Initialize(gameStateManager);
        }

        public void StartGame()
        {
            if (!isInitialized) InitializeGame();

            StartGameSystems();
            currentMatchCount = 0;
        }

        private void InitializeGame()
        {
            gridController.PreInitialize(levelData, cubePool);

            cubeFactory = new CubeFactory(cubeDatabase, levelData, cubePool);
            matchFinder = new MatchFinder(gridController.Grid);
            powerupManager = new PowerupManager(levelData, cubeFactory);
            animationController = new AnimationController();
            gravityProcessor = new GravityProcessor(gridController, animationController);
            matchProcessor = new MatchProcessor(matchFinder, levelData.minMatchCount, powerupManager, gravityProcessor, animationController);

            gameStateManager.Initialize(levelData);

            gridController.Initialize(cubeFactory, animationController, matchProcessor, levelData);

            SubscribeToEvents();

            isInitialized = true;
        }

        private void SubscribeToEvents()
        {
            CellEvents.OnCellClicked += OnCellClicked;
            matchProcessor.OnMatchProcessed += CheckWinCondition;
        }
        private void StartGameSystems()
        {
            gameStateManager.StartGame();
            gridController.CheckAndShuffleIfNeeded();
        }
        private void OnCellClicked(Cell cell)
        {
            if (gameStateManager.GetCurrentState() != GameState.Playing) return;

            gameStateManager.OnMovePerformed();
        }
        private void CheckWinCondition()
        {
            currentMatchCount++;
            if (currentMatchCount >= levelData.targetMatchCount)
            {
                gameStateManager.OnLevelCompleted();
            }
        }
    }
}