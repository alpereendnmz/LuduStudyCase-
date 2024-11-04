using System;
using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class AnimationController
    {
        private readonly MovementAnimationController movementController;
        private readonly PowerUpAnimationController powerUpController;
        private readonly ShuffleAnimationController shuffleController;
        private readonly ObstacleAnimationController obstacleController;
        private readonly MatchAnimationController matchController;

        public AnimationController()
        {
            movementController = new MovementAnimationController();
            powerUpController = new PowerUpAnimationController();
            shuffleController = new ShuffleAnimationController();
            obstacleController = new ObstacleAnimationController();
            matchController = new MatchAnimationController();
        }

        public Sequence CreateMatchSequence(List<BaseCube> matchedCubes, Vector2Int originPosition)
        {
            return matchController.CreateMatchSequence(matchedCubes, originPosition);
        }
        public Sequence CreateDropSequence(List<BaseCube> moverCubes)
        {
            return movementController.CreateDropSequence(moverCubes);
        }
        public Sequence CreateSpawnSequence(List<BaseCube> cubes, Action onSpawnComplete)
        {
            return movementController.CreateSpawnSequence(cubes, onSpawnComplete);
        }

        public Sequence CreatePowerUpSequence(List<BaseCube> affectedCubes, PowerUpType powerUpType, BaseCube originCube, Vector2Int originPosition)
        {
            return powerUpController.CreatePowerUpSequence(affectedCubes, powerUpType, originCube, originPosition);
        }

        public Sequence CreateShuffleSequence(BaseCube[] shuffledCubes)
        {
            return shuffleController.CreateShuffleSequence(shuffledCubes);
        }
        public Sequence CreateObstaclePowerUpSequence(List<BaseCube> affectedCubes, PowerUpType powerUpType, Vector2Int originPosition)
        {
            return obstacleController.CreateObstaclePowerUpSequence(affectedCubes, powerUpType, originPosition);
        }
    }
}