using System.Collections.Generic;
using Match2.Core.Cube;
using Match2.Core.Grid;
namespace Match2.Core.Services.Matching
{
    public class CubeProcessor
    {
        public void ProcessMatchedCells(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                var cube = cell.CurrentCube;
                if (cube == null) continue;

                if (cube is ObstacleCube obstacle)
                {
                    ProcessObstacle(obstacle, cell);
                }
                else
                {
                    cell.ClearCube();
                }
            }
        }

        private void ProcessObstacle(ObstacleCube obstacle, Cell cell)
        {
            if (obstacle.CanTakeDamage())
            {
                obstacle.TakeDamage(1);
                if (obstacle.GetHealth() <= 0)
                {
                    cell.ClearCube();
                }
            }
        }
    }
}