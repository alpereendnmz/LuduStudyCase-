using Match2.Core.Cube;
using Match2.Core.Data;
using UnityEngine;
namespace Match2.Core.Grid
{
    public class GridSetupHandler
    {
        private readonly LevelData levelData;
        private readonly GameObject cellPrefab;
        private readonly RectTransform gridContainer;
        private readonly CubePool cubePool;
        private readonly DynamicBackgroundManager backgroundManager;

        private Cell[,] grid;

        public Cell[,] Grid => grid;
        public GridSetupHandler(LevelData levelData, GameObject cellPrefab, RectTransform gridContainer, CubePool cubePool, DynamicBackgroundManager backgroundManager)
        {
            this.levelData = levelData;
            this.cellPrefab = cellPrefab;
            this.gridContainer = gridContainer;
            this.cubePool = cubePool;
            this.backgroundManager = backgroundManager;
        }


        public void Setup()
        {
            SetupContainer();
            CreateGrid();
            MarkEmptyCells();

            backgroundManager.Initialize(levelData, grid);
        }

        private void SetupContainer()
        {
            float totalWidth = (levelData.width * levelData.cellSize) +
                              ((levelData.width - 1) * levelData.cellSpacing);
            float totalHeight = (levelData.height * levelData.cellSize) +
                               ((levelData.height - 1) * levelData.cellSpacing);

            gridContainer.sizeDelta = new Vector2(totalWidth, totalHeight);
        }

        private void CreateGrid()
        {
            grid = new Cell[levelData.width, levelData.height];

            for (int x = 0; x < levelData.width; x++)
            {
                for (int y = 0; y < levelData.height; y++)
                {
                    CreateCell(new Vector2Int(x, y));
                }
            }
        }

        private void CreateCell(Vector2Int position)
        {
            var cellObject = Object.Instantiate(cellPrefab, gridContainer);
            cellObject.name = $"Cell_{position.x}_{position.y}";

            var cell = cellObject.GetComponent<Cell>();
            var rectTransform = cell.RectTransform;

            SetupCellTransform(rectTransform, position);

            grid[position.x, position.y] = cell;
            cell.Initialize(cubePool, position, false);
        }

        private void SetupCellTransform(RectTransform rectTransform, Vector2Int position)
        {
            rectTransform.sizeDelta = new Vector2(levelData.cellSize, levelData.cellSize);
            rectTransform.pivot = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;

            float xPos = position.x * (levelData.cellSize + levelData.cellSpacing);
            float yPos = position.y * (levelData.cellSize + levelData.cellSpacing);
            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }

        private void MarkEmptyCells()
        {
            foreach (var deadPos in levelData.deadCells)
            {
                if (!IsValidPosition(deadPos)) return;
                grid[deadPos.x, deadPos.y].Initialize(null, deadPos, true);
            }
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < levelData.width
                                        &&
                   position.y >= 0 && position.y < levelData.height;
        }
    }
}