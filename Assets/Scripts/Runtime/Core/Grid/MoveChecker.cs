using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Grid
{
    public class MoveChecker
    {
        private readonly Cell[,] grid;
        private readonly int width;
        private readonly int height;
        private readonly int minMatchCount;

        private static readonly Vector2Int[] directions = new[]
        {
        Vector2Int.right,
        Vector2Int.up
    };

        public MoveChecker(Cell[,] grid, int minMatchCount)
        {
            this.grid = grid;
            this.width = grid.GetLength(0);
            this.height = grid.GetLength(1);
            this.minMatchCount = minMatchCount;
        }

        public bool HasPossibleMatches()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (CheckCell(x, y))
                        return true;
                }
            }

            return false;
        }

        private bool CheckCell(int x, int y)
        {
            var cell = grid[x, y];
            if (cell.IsDeadCell || cell.CurrentCube == null || !(cell.CurrentCube is ColorCube currentCube))
                return false;

            ColorType currentColor = currentCube.GetColorType();

            foreach (var dir in directions)
            {
                if (dir == Vector2Int.right && x < width - 2)
                {
                    if (CheckHorizontalMatch(x, y, currentColor))
                        return true;
                }
                else if (dir == Vector2Int.up && y < height - 2)
                {
                    if (CheckVerticalMatch(x, y, currentColor))
                        return true;
                }
            }

            return false;
        }

        private bool CheckHorizontalMatch(int x, int y, ColorType color)
        {
            int matchCount = 1;
            int checkX = x;

            while (++checkX < width)
            {
                var nextCell = grid[checkX, y];
                if (IsMatchingCell(nextCell, color))
                    matchCount++;
                else
                    break;

                if (matchCount >= minMatchCount)
                    return true;
            }

            return false;
        }

        private bool CheckVerticalMatch(int x, int y, ColorType color)
        {
            int matchCount = 1;
            int checkY = y;

            while (++checkY < height)
            {
                var nextCell = grid[x, checkY];
                if (IsMatchingCell(nextCell, color))
                    matchCount++;
                else
                    break;

                if (matchCount >= minMatchCount)
                    return true;
            }

            return false;
        }

        private bool IsMatchingCell(Cell cell, ColorType color)
        {
            if (cell.IsDeadCell || cell.CurrentCube == null || cell.CurrentCube is not ColorCube cube)
                return false;

            return cube.GetColorType() == color;
        }
    }
}