using Match2.Core.Cube;
using UnityEngine;
namespace Match2.Core.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform cubeParent;

        private CubePool cubePool;
        private Vector2Int gridPosition;
        private BaseCube currentCube;
        private bool isDeadCell;

        public Vector2Int GridPosition => gridPosition;
        public BaseCube CurrentCube => currentCube;
        public bool IsDeadCell => isDeadCell;
        public RectTransform RectTransform => rectTransform;

        public void Initialize(CubePool cubePool, Vector2Int position, bool isDeadCell = false)
        {
            gridPosition = position;
            cubeParent.sizeDelta = rectTransform.sizeDelta;

            this.isDeadCell = isDeadCell;
            this.cubePool = cubePool;
        }

        public bool SetCube(BaseCube cube, bool isInitial = false)
        {
            currentCube = cube;

            if (cube == null) return true;

            currentCube.Setup(this);

            var cubeRect = cube.transform as RectTransform;
            cubeRect.SetParent(cubeParent);
            cubeRect.sizeDelta = cubeParent.sizeDelta;
            cubeRect.localScale = Vector3.one;

            if (isInitial) cubeRect.anchoredPosition = Vector2.zero;

            return true;
        }
        public void ClearCube()
        {
            if (currentCube == null) return;

            cubePool.ReleaseCube(currentCube);
            SetCube(null);
        }
    }
}