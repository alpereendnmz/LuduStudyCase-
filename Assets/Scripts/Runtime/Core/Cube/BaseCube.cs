using Match2.Core.Grid;
using Match2.Core.Types;
using Match2.Core.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Match2.Core.Cube
{
    public abstract class BaseCube : MonoBehaviour
    {
        [SerializeField] protected Button button;
        protected GridController gridController;
        private Cell currentCell;

        public Cell CurrentCell
        {
            get => currentCell;
            set => currentCell = value;
        }
        public virtual void Initialize(GridController gridController, PowerUpType powerUpType)
        {
            this.gridController = gridController;

            button.onClick.AddListener(OnCubeClicked);
        }
        public virtual void Initialize(int health)
        {
            button.onClick.AddListener(OnCubeClicked);
        }
        public virtual void Initialize(ColorType colorType)
        {
            button.onClick.AddListener(OnCubeClicked);
        }
        public virtual void Setup(Cell cell)
        {
            currentCell = cell;
        }
        protected virtual void OnCubeClicked()
        {
            CellEvents.CellClicked(currentCell);
        }
    }
}
