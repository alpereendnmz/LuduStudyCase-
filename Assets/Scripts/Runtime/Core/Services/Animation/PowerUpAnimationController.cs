using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class PowerUpAnimationController : BaseAnimationController
    {
        private const float SHAKE_STRENGTH = 20f;
        private const float SHAKE_DURATION = 0.2f;
        private const int VIBRATO = 45;


        public Sequence CreatePowerUpSequence(List<BaseCube> affectedCubes, PowerUpType powerUpType, BaseCube originCube, Vector2Int originPosition)
        {
            var sequence = DOTween.Sequence();

            switch (powerUpType)
            {
                case PowerUpType.HorizontalRocket:
                    sequence.Append(CreateHorizontalRocketSequence(affectedCubes, originCube, originPosition));
                    break;
                case PowerUpType.VerticalRocket:
                    sequence.Append(CreateVerticalRocketSequence(affectedCubes, originCube, originPosition));
                    break;
                case PowerUpType.Bomb:
                    sequence.Append(CreateBombSequence(affectedCubes, originCube));
                    break;
            }

            sequence.Append(CreateDestroySequence(affectedCubes));
            return sequence;
        }

        private Sequence CreateHorizontalRocketSequence(List<BaseCube> affectedCubes, BaseCube originCube, Vector2Int originPos)
        {
            var sequence = DOTween.Sequence();

            var originRect = originCube.transform as RectTransform;
            originCube.transform.SetParent(originCube.transform.root);
            sequence.Append(originRect.DOScale(POWER_UP_ACTIVE_SCALE, SHAKE_DURATION));

            foreach (var cube in affectedCubes)
            {
                var cubeRect = cube.transform as RectTransform;

                if (cubeRect == null) continue;

                float delay = SHAKE_DURATION + Mathf.Abs(cube.CurrentCell.GridPosition.x - originPos.x) * 0.05f;
                sequence.Insert(delay, cubeRect.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH, VIBRATO));
            }

            return sequence;
        }

        private Sequence CreateVerticalRocketSequence(List<BaseCube> affectedCubes, BaseCube originCube, Vector2Int originPos)
        {
            var sequence = DOTween.Sequence();

            var originRect = originCube.transform as RectTransform;
            originCube.transform.SetParent(originCube.transform.root);
            sequence.Append(originRect.DOScale(POWER_UP_ACTIVE_SCALE, SHAKE_DURATION));

            foreach (var cube in affectedCubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                float delay = SHAKE_DURATION + Mathf.Abs(cube.CurrentCell.GridPosition.y - originPos.y) * 0.05f;
                sequence.Insert(delay, cubeRect.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH, VIBRATO));
            }

            return sequence;
        }

        private Sequence CreateBombSequence(List<BaseCube> affectedCubes, BaseCube originCube)
        {
            var sequence = DOTween.Sequence();

            var originRect = originCube.transform as RectTransform;
            originCube.transform.SetParent(originCube.transform.root);

            sequence.Append(originRect.DOScale(POWER_UP_ACTIVE_SCALE * 2, SHAKE_DURATION));
            sequence.Append(originRect.DOLocalRotate(new Vector3(0, 0, 720), SHAKE_DURATION, RotateMode.FastBeyond360));


            foreach (var cube in affectedCubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect == null) continue;

                sequence.Join(cubeRect.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH, VIBRATO));
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