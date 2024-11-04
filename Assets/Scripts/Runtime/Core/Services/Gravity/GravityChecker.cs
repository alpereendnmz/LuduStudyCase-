using Match2.Core.Cube;
using Match2.Core.Grid;
using Match2.Core.Interfaces;

namespace Match2.Core.Services.Gravity
{
    public class GravityChecker
    {
        private readonly Cell[,] grid;
        private readonly int height;

        public GravityChecker(Cell[,] grid)
        {
            this.grid = grid;
            height = grid.GetLength(1);
        }

        public BaseCube FindMovableCubeAbove(int column, int startRow)
        {
            for (int row = startRow + 1; row < height; row++)
            {
                var cell = grid[column, row];
                if (cell.IsDeadCell || cell.CurrentCube == null)
                    continue;

                var cube = cell.CurrentCube;
                if (cube is IMoveable moveable)
                {
                    return moveable.CanMove() ? cube : null;
                }

                return null;
            }

            return null;
        }

        public bool HasImmovableBlockAbove(int column, int row)
        {
            for (int currentRow = row + 1; currentRow < height; currentRow++)
            {
                var cell = grid[column, currentRow];
                if (cell.IsDeadCell) continue;

                if (cell.CurrentCube != null)
                {
                    var moveable = cell.CurrentCube as IMoveable;
                    if (moveable == null || !moveable.CanMove())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CanFillCell(Cell cell, int column, int row)
        {
            return !cell.IsDeadCell &&
                   cell.CurrentCube == null &&
                   !HasImmovableBlockAbove(column, row);
        }
    }
}