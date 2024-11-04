using System.Collections.Generic;
using Match2.Core.Grid;

namespace Match2.Core.Cube
{
    public class HorizontalRocketPowerUpCube : PowerUpCube
    {
        public override List<Cell> GetAffectedCells()
        {
            var affectedCells = new List<Cell>();
            var currentPos = CurrentCell.GridPosition;
            var grid = gridController.Grid;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                var cell = grid[x, currentPos.y];
                if (!cell.IsDeadCell && cell.CurrentCube != null)
                {
                    affectedCells.Add(cell);
                }
            }

            return affectedCells;
        }

    }
}

