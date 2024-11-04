using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using UnityEngine;
namespace Match2.Core.Services.Animation
{
    public class ShuffleAnimationController : BaseAnimationController
    {
        public Sequence CreateShuffleSequence(BaseCube[] shuffledCubes)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(CreateShuffleLiftTweens(shuffledCubes));
            sequence.Append(CreateShuffleMoveTweens(shuffledCubes));
            sequence.Append(CreateShuffleDropTweens(shuffledCubes));

            return sequence;
        }

        private Sequence CreateShuffleLiftTweens(BaseCube[] cubes)
        {
            var sequence = DOTween.Sequence();
            float liftHeight = 30f;

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect != null)
                {
                    var lift = cubeRect.DOAnchorPosY(liftHeight, SHUFFLE_DURATION * 0.3f)
                                     .SetEase(Ease.OutQuad);
                    sequence.Join(lift);
                }
            }

            return sequence;
        }

        private Sequence CreateShuffleMoveTweens(BaseCube[] cubes)
        {
            var sequence = DOTween.Sequence();

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect != null)
                {
                    var move = cubeRect.DOAnchorPosX(0, SHUFFLE_DURATION * 0.4f)
                                     .SetEase(Ease.InOutQuad);
                    sequence.Join(move);
                }
            }

            return sequence;
        }

        private Sequence CreateShuffleDropTweens(BaseCube[] cubes)
        {
            var sequence = DOTween.Sequence();

            foreach (var cube in cubes)
            {
                var cubeRect = cube.transform as RectTransform;
                if (cubeRect != null)
                {
                    var drop = cubeRect.DOAnchorPosY(0, SHUFFLE_DURATION * 0.3f)
                                     .SetEase(Ease.OutBounce);
                    sequence.Join(drop);
                }
            }

            return sequence;
        }
    }
}