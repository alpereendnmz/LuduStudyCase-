using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class ObstacleAnimationController : BaseAnimationController
    {
        private const float OBSTACLE_SHAKE_STRENGTH = 5f;

        public Sequence CreateObstaclePowerUpSequence(List<BaseCube> affectedCubes, PowerUpType powerUpType, Vector2Int originPosition)
        {
            var sequence = DOTween.Sequence();

            switch (powerUpType)
            {
                case PowerUpType.HorizontalRocket:
                case PowerUpType.VerticalRocket:
                    sequence.Append(CreateObstacleRocketSequence(affectedCubes, originPosition));
                    break;
                case PowerUpType.Bomb:
                    sequence.Append(CreateObstacleBombSequence(affectedCubes, originPosition));
                    break;
            }

            sequence.Join(CreateObstacleDestroySequence(affectedCubes));
            return sequence;
        }

        private Sequence CreateObstacleRocketSequence(List<BaseCube> affectedCubes, Vector2Int origin)
        {
            var sequence = DOTween.Sequence();
            float shakeDuration = POWER_UP_DURATION * 0.5f;

            foreach (var cube in affectedCubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                float delay = Vector2Int.Distance(cube.CurrentCell.GridPosition, origin) * 0.05f;

                var shake = cubeRect.DOShakePosition(shakeDuration, OBSTACLE_SHAKE_STRENGTH)
                    .SetDelay(delay)
                    .SetEase(Ease.OutQuad);

                var scale = cubeRect.DOPunchScale(Vector3.one * 0.2f, shakeDuration, 2, 0.5f)
                    .SetDelay(delay);

                sequence.Join(shake);
                sequence.Join(scale);
            }

            return sequence;
        }

        private Sequence CreateObstacleBombSequence(List<BaseCube> affectedCubes, Vector2Int origin)
        {
            var sequence = DOTween.Sequence();
            float shakeDuration = POWER_UP_DURATION * 0.6f;

            foreach (var cube in affectedCubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                var distance = Vector2Int.Distance(cube.CurrentCell.GridPosition, origin);
                float delay = distance * 0.05f;
                float shakeStrength = OBSTACLE_SHAKE_STRENGTH * (1f - distance * 0.2f);

                var shake = cubeRect.DOShakePosition(shakeDuration, shakeStrength)
                    .SetDelay(delay)
                    .SetEase(Ease.OutQuad);

                var scale = cubeRect.DOPunchScale(Vector3.one * 0.3f, shakeDuration, 3, 0.5f)
                    .SetDelay(delay);

                sequence.Join(shake);
                sequence.Join(scale);
            }

            return sequence;
        }

        private Sequence CreateObstacleDestroySequence(List<BaseCube> cubes)
        {
            var sequence = DOTween.Sequence();
            float shakeDuration = POWER_UP_DURATION * 2;

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                var rotate = cubeRect.DOShakePosition(shakeDuration, OBSTACLE_SHAKE_STRENGTH * 1.2f, 45, 90);
                sequence.Join(rotate);
            }

            return sequence;
        }
    }
}