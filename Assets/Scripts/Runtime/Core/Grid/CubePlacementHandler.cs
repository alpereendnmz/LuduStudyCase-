using Match2.Core.Cube;
using Match2.Core.Data;
using UnityEngine;

namespace Match2.Core.Grid
{
    public class CubePlacementHandler
    {
        private readonly Cell[,] grid;
        private readonly LevelData levelData;
        private readonly CubeFactory cubeFactory;

        public CubePlacementHandler(Cell[,] grid, LevelData levelData, CubeFactory cubeFactory)
        {
            this.grid = grid;
            this.levelData = levelData;
            this.cubeFactory = cubeFactory;
        }

        public void PlaceInitialCubes()
        {
            PlaceInitialObstacles();
            CreateInitialCubes();
        }

        private void PlaceInitialObstacles()
        {
            foreach (var obstaclePos in levelData.initialObstacles)
            {
                if (IsValidPosition(obstaclePos.position))
                {
                    var cell = grid[obstaclePos.position.x, obstaclePos.position.y];
                    if (cell.IsDeadCell) continue;

                    var obstacle = cubeFactory.CreateObstacle(obstaclePos.health);
                    cell.SetCube(obstacle, true);
                }
            }
        }

        private void CreateInitialCubes()
        {
            for (int x = 0; x < levelData.width; x++)
            {
                for (int y = 0; y < levelData.height; y++)
                {
                    var cell = grid[x, y];
                    if (cell.IsDeadCell || cell.CurrentCube != null) continue;

                    CreateRandomCube(cell, true);
                }
            }
        }

        public ColorCube CreateRandomCube(Cell cell, bool isInitial)
        {
            if (cell.IsDeadCell || cell.CurrentCube != null) return null;

            var cube = cubeFactory.CreateRandomColorCube();
            cell.SetCube(cube, isInitial);
            return cube;
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < levelData.width &&
                   position.y >= 0 && position.y < levelData.height;
        }
    }
}