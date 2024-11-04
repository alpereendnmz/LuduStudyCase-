using System.Collections.Generic;
using Match2.Core.Interfaces;
namespace Match2.Core.Grid
{
    public class ShuffleChecker
    {
        private readonly Cell[,] grid;
        private readonly int width;
        private readonly int height;
        private readonly List<Cell> blockedCells;

        public ShuffleChecker(Cell[,] grid)
        {
            this.grid = grid;
            width = grid.GetLength(0);
            height = grid.GetLength(1);
            blockedCells = new List<Cell>();
        }

        public void MarkBlockedCells()
        {
            blockedCells.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = grid[x, y];
                    if (cell.IsDeadCell)
                    {
                        blockedCells.Add(cell);
                        continue;
                    }

                    var cube = cell.CurrentCube;
                    if (cube == null) continue;

                    var moveable = cube as IMoveable;
                    if (moveable == null || !moveable.CanMove())
                    {
                        blockedCells.Add(cell);
                        MarkCellsBelow(x, y);
                    }
                }
            }
        }

        private void MarkCellsBelow(int x, int y)
        {
            for (int belowY = y - 1; belowY >= 0; belowY--)
            {
                var cell = grid[x, belowY];
                if (!blockedCells.Contains(cell))
                {
                    blockedCells.Add(cell);
                }
            }
        }

        public bool IsCellAvailable(Cell cell)
        {
            return !blockedCells.Contains(cell) && cell.CurrentCube == null;
        }

        public bool CanCollectFromCell(Cell cell)
        {
            return !blockedCells.Contains(cell) && cell.CurrentCube != null;
        }

    }
}