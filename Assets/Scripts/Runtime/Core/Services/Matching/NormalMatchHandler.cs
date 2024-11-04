using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match2.Core.Grid;
using Match2.Core.Services.Animation;
using Match2.Core.Services.Gravity;
namespace Match2.Core.Services.Matching
{
    public class NormalMatchHandler
    {
        private readonly int minMatchCount;
        private readonly PowerupManager powerupManager;
        private readonly GravityProcessor gravityProcessor;
        private readonly AnimationController animationController;
        private readonly CubeProcessor cubeProcessor;

        public NormalMatchHandler(
            int minMatchCount,
            PowerupManager powerupManager,
            GravityProcessor gravityProcessor,
            AnimationController animationController)
        {
            this.minMatchCount = minMatchCount;
            this.powerupManager = powerupManager;
            this.gravityProcessor = gravityProcessor;
            this.animationController = animationController;
            this.cubeProcessor = new CubeProcessor();
        }

        public void Process(List<Cell> matches, Cell originCell, Action onMatchProcessed)
        {
            if (matches.Count < minMatchCount) return;

            var originCube = originCell.CurrentCube;
            var matchedCubes = matches.Select(m => m.CurrentCube).ToList();

            animationController.CreateMatchSequence(matchedCubes, originCell.GridPosition)
                .OnComplete(() =>
                {
                    cubeProcessor.ProcessMatchedCells(matches);
                    powerupManager.CheckForPowerUP(matches.Count, originCell, originCube);
                    gravityProcessor.StartGravitiyProcess();
                    onMatchProcessed?.Invoke();
                });
        }
    }
}