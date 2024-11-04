using System;
using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class MovementAnimationController : BaseAnimationController
    {
        public Sequence CreateDropSequence(List<BaseCube> moverCubes)
        {
            var sequence = DOTween.Sequence();
            foreach (BaseCube cube in moverCubes)
            {
                sequence.Join(CreateDropTween(cube));
            }
            return sequence;
        }

        private Tween CreateDropTween(BaseCube cube)
        {
            return CreateBaseTween(cube, Vector2.zero, MOVE_DURATION, Ease.OutBounce);
        }

        public Sequence CreateSpawnSequence(List<BaseCube> cubes, Action onSpawnComplete)
        {
            var sequence = DOTween.Sequence();

            foreach (var cube in cubes)
            {
                sequence.Join(CreateSpawnTween(cube));
            }
            sequence.OnComplete(() => onSpawnComplete?.Invoke());
            return sequence;
        }

        private Tween CreateSpawnTween(BaseCube cube)
        {
            var cubeRect = cube.transform as RectTransform;
            if (cubeRect == null) return null;

            cubeRect.anchoredPosition = new Vector2(0, 200f);
            return cubeRect.DOAnchorPos(Vector2.zero, SPAWN_DURATION).SetEase(Ease.OutBounce);
        }
    }
}