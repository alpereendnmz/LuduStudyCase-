using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class MatchAnimationController : BaseAnimationController
    {
        private const float SHAKE_STRENGTH = 30f;
        private const float SHAKE_DURATION = 0.1f;
        private const int VIBRATO = 45;


        public Sequence CreateMatchSequence(List<BaseCube> matchedCubes, Vector2Int originPosition)
        {
            var sequence = DOTween.Sequence();
            var normalCubes = new List<BaseCube>(matchedCubes.Count);
            var obstacleCubes = new List<BaseCube>(matchedCubes.Count);

            foreach (var cube in matchedCubes)
            {
                if (cube == null) continue;
                (cube is ObstacleCube ? obstacleCubes : normalCubes).Add(cube);
            }

            if (normalCubes.Count > 0)
            {
                var normalSequence = CreateNormalMatchSequence(normalCubes);
                sequence.Join(normalSequence);
            }

            if (obstacleCubes.Count > 0)
            {
                var obstacleSequence = CreateObstacleMatchSequence(obstacleCubes);
                sequence.Join(obstacleSequence);
            }

            sequence.Append(CreateDestroySequence(matchedCubes));
            return sequence;
        }

        private Sequence CreateNormalMatchSequence(List<BaseCube> cubes)
        {
            var sequence = DOTween.Sequence();

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                sequence.Join(cubeRect.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH, VIBRATO));
            }

            return sequence;
        }

        private Sequence CreateObstacleMatchSequence(List<BaseCube> cubes)
        {
            var sequence = DOTween.Sequence();

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                sequence.Join(cubeRect.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH * 0.7f, VIBRATO));
            }

            return sequence;
        }

        private Sequence CreateDestroySequence(List<BaseCube> cubes)
        {
            var sequence = DOTween.Sequence();
            float fadeOutDuration = POWER_UP_DURATION;

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                var shrink = cubeRect.DOScale(Vector3.zero, fadeOutDuration)
                    .SetEase(Ease.InBack);

                sequence.Join(shrink);
            }

            return sequence;
        }
    }
}