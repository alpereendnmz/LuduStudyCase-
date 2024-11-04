using System.Collections.Generic;
using Match2.Core.Grid;

namespace Match2.Core.Cube
{
    public class VerticalRocketPowerUpCube : PowerUpCube
    {
        public override List<Cell> GetAffectedCells()
        {
            var affectedCells = new List<Cell>();
            var currentPos = CurrentCell.GridPosition;
            var grid = gridController.Grid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var cell = grid[currentPos.x, y];

                if (cell.IsDeadCell || cell.CurrentCube == null) continue;
                affectedCells.Add(cell);
            }

            return affectedCells;
        }
    }
}

