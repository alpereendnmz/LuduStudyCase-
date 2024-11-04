using Match2.Core.Data;
using UnityEngine;
using UnityEngine.UI;
namespace Match2.Core.Grid
{
    public class DynamicBackgroundManager : MonoBehaviour
    {
        [SerializeField] private GameObject backgroundPrefab;
        [SerializeField] private RectTransform backgroundContainer;
        private GameObject[,] backgrounds;

        public void Initialize(LevelData levelData, Cell[,] grid)
        {
            backgrounds = new GameObject[levelData.width, levelData.height];
            CreateBackgrounds(levelData, grid);
        }

        private void CreateBackgrounds(LevelData levelData, Cell[,] grid)
        {
            float totalWidth = (levelData.width * levelData.cellSize) +
                              ((levelData.width - 1) * levelData.cellSpacing);
            float totalHeight = (levelData.height * levelData.cellSize) +
                              ((levelData.height - 1) * levelData.cellSpacing);

            backgroundContainer.sizeDelta = new Vector2(totalWidth, totalHeight);

            for (int x = 0; x < levelData.width; x++)
            {
                for (int y = 0; y < levelData.height; y++)
                {
                    if (!grid[x, y].IsDeadCell)
                    {
                        CreateBackground(x, y, levelData);
                    }
                }
            }
        }

        private void CreateBackground(int x, int y, LevelData levelData)
        {
            GameObject background = Instantiate(backgroundPrefab, backgroundContainer);
            backgrounds[x, y] = background;

            RectTransform rectTransform = background.GetComponent<RectTransform>();
            rectTransform.pivot = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;

            float xPos = x * (levelData.cellSize + levelData.cellSpacing);
            float yPos = y * (levelData.cellSize + levelData.cellSpacing);

            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
            rectTransform.sizeDelta = new Vector2(levelData.cellSize, levelData.cellSize);

        }

    }
}