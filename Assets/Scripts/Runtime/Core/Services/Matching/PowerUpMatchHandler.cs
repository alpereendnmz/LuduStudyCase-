using System;
using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Grid;
using Match2.Core.Services.Animation;
using Match2.Core.Services.Gravity;
namespace Match2.Core.Services.Matching
{
    public class PowerUpMatchHandler
    {
        private readonly GravityProcessor gravityProcessor;
        private readonly AnimationController animationController;
        private readonly CubeProcessor cubeProcessor;
        private readonly CubeGrouper cubeGrouper;

        public PowerUpMatchHandler(
            GravityProcessor gravityProcessor, AnimationController animationController)
        {
            this.gravityProcessor = gravityProcessor;
            this.animationController = animationController;
            this.cubeProcessor = new CubeProcessor();
            this.cubeGrouper = new CubeGrouper();
        }

        public void Process(List<Cell> matches, Cell originCell, Action onMatchProcessed)
        {
            if (matches.Count == 0) return;

            var powerUp = originCell.CurrentCube as PowerUpCube;
            var (normalCubes, obstacleCubes) = cubeGrouper.GroupCubes(matches);

            var sequence = DOTween.Sequence();

            if (normalCubes.Count > 0)
            {
                sequence.Join(animationController.CreatePowerUpSequence(
                    normalCubes,
                    powerUp.GetPowerUpType(),
                    originCell.CurrentCube,
                    originCell.GridPosition));
            }

            if (obstacleCubes.Count > 0)
            {
                sequence.Join(animationController.CreateObstaclePowerUpSequence(
                    obstacleCubes,
                    powerUp.GetPowerUpType(),
                    originCell.GridPosition));
            }

            sequence.OnComplete(() =>
            {
                cubeProcessor.ProcessMatchedCells(matches);
                gravityProcessor.StartGravitiyProcess();
                onMatchProcessed?.Invoke();
            });
        }
    }
}