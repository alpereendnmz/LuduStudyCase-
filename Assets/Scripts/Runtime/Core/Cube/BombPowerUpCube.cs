using System.Collections.Generic;
using Match2.Core.Grid;
using UnityEngine;

namespace Match2.Core.Cube
{
    public class BombPowerUpCube : PowerUpCube
    {
        private static readonly Vector2Int[] bombArea = {
       new(-1, 1),  new(0, 1),  new(1, 1),
       new(-1, 0),  new(0, 0),  new(1, 0),
       new(-1, -1), new(0, -1), new(1, -1)
   };

        public override List<Cell> GetAffectedCells()
        {
            var affectedCells = new List<Cell>();

            foreach (var offset in bombArea)
            {
                var checkPos = CurrentCell.GridPosition + offset;
                var cell = TryGetCell(checkPos);

                if (cell != null && !cell.IsDeadCell && cell.CurrentCube != null)
                {
                    affectedCells.Add(cell);
                }
            }

            return affectedCells;
        }

        private Cell TryGetCell(Vector2Int pos)
        {
            var grid = gridController.Grid;
            bool isInGrid = pos.x >= 0 && pos.x < grid.GetLength(0) &&
                           pos.y >= 0 && pos.y < grid.GetLength(1);

            return isInGrid ? grid[pos.x, pos.y] : null;
        }
    }
}
