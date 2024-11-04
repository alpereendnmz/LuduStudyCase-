using System;
using System.Collections.Generic;
using Match2.Core.Cube;
using Match2.Core.Interfaces;
namespace Match2.Core.Grid
{
    public class GridShuffler
    {
        private readonly Cell[,] grid;
        private readonly BaseCube[] moveableCubes;
        private readonly ShuffleChecker shuffleChecker;
        private readonly int width;
        private readonly int height;
        private int cubeCount;

        public GridShuffler(Cell[,] grid)
        {
            this.grid = grid;
            this.width = grid.GetLength(0);
            this.height = grid.GetLength(1);
            this.moveableCubes = new BaseCube[width * height];
            this.shuffleChecker = new ShuffleChecker(grid);
        }

        public BaseCube[] ShuffleGrid()
        {
            shuffleChecker.MarkBlockedCells();
            CollectCubes();

            if (cubeCount == 0) return Array.Empty<BaseCube>();

            ShuffleCubes();
            return RedistributeCubes();
        }

        private void CollectCubes()
        {
            cubeCount = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = grid[x, y];
                    if (!shuffleChecker.CanCollectFromCell(cell)) continue;

                    var cube = cell.CurrentCube;
                    if (cube is IMoveable moveable && moveable.CanMove())
                    {
                        moveableCubes[cubeCount++] = cube;
                        cell.SetCube(null);
                    }
                }
            }
        }

        private void ShuffleCubes()
        {
            for (int i = cubeCount - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                if (i != randomIndex)
                {
                    var temp = moveableCubes[i];
                    moveableCubes[i] = moveableCubes[randomIndex];
                    moveableCubes[randomIndex] = temp;
                }
            }
        }

        private BaseCube[] RedistributeCubes()
        {
            var availableCells = new List<Cell>();
            var movedCubes = new List<BaseCube>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = grid[x, y];
                    if (shuffleChecker.IsCellAvailable(cell))
                    {
                        availableCells.Add(cell);
                    }
                }
            }
            if (availableCells.Count != cubeCount) return Array.Empty<BaseCube>();

            for (int i = 0; i < cubeCount; i++)
            {
                var cube = moveableCubes[i];
                var targetCell = availableCells[i];
                var previousCell = cube.CurrentCell;

                targetCell.SetCube(cube);
                if (previousCell != targetCell)
                {
                    movedCubes.Add(cube);
                }
            }

            return movedCubes.ToArray();
        }
    }
}