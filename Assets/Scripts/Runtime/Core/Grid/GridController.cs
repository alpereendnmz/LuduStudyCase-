using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Data;
using Match2.Core.Services.Animation;
using Match2.Core.Services.Matching;
using UnityEngine;
namespace Match2.Core.Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private RectTransform gridContainer;
        [SerializeField] private DynamicBackgroundManager backgroundManager;
        private GridSetupHandler gridSetupHandler;
        private MoveChecker moveChecker;
        private GridShuffler gridShuffler;
        private AnimationController animationController;

        public MatchProcessor MatchProcessor { get; private set; }
        public CubePlacementHandler CubePlacementHandler { get; private set; }
        public Cell[,] Grid => gridSetupHandler.Grid;

        public void PreInitialize(LevelData levelData, CubePool cubePool)
        {
            gridSetupHandler = new GridSetupHandler(levelData, cellPrefab, gridContainer, cubePool, backgroundManager);
            gridSetupHandler.Setup();

            moveChecker = new MoveChecker(Grid, levelData.minMatchCount);
            gridShuffler = new GridShuffler(Grid);
        }

        public void Initialize(CubeFactory cubeFactory, AnimationController animationController, MatchProcessor matchProcessor, LevelData levelData)
        {
            this.animationController = animationController;
            this.MatchProcessor = matchProcessor;

            CubePlacementHandler = new CubePlacementHandler(Grid, levelData, cubeFactory);
            CubePlacementHandler.PlaceInitialCubes();
        }

        public void CheckAndShuffleIfNeeded()
        {
            if (!moveChecker.HasPossibleMatches())
            {
                var movedCubes = gridShuffler.ShuffleGrid();
                var sequence = animationController.CreateShuffleSequence(movedCubes);
                sequence.OnComplete(() =>
                {
                    if (!moveChecker.HasPossibleMatches())
                        CheckAndShuffleIfNeeded();
                });
            }
        }
    }
}