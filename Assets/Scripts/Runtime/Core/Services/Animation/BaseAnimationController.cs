using DG.Tweening;
using Match2.Core.Cube;
using UnityEngine;

namespace Match2.Core.Services.Animation
{
    public abstract class BaseAnimationController
    {
        protected const float MOVE_DURATION = 0.1f;
        protected const float SPAWN_DURATION = 0.2f;
        protected const float SHUFFLE_DURATION = 0.3f;
        protected const float POWER_UP_DURATION = 0.2f;
        protected const float POWER_UP_ACTIVE_SCALE = 1.5f;

        protected const float EXPLOSION_SCALE = 1.1f;

        protected Tween CreateBaseTween(BaseCube cube, Vector2 targetPosition, float duration, Ease ease)
        {
            var cubeRect = cube.transform as RectTransform;
            if (cubeRect == null) return null;

            return cubeRect.DOAnchorPos(targetPosition, duration).SetEase(ease);
        }
    }
}