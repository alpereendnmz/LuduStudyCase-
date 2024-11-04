using System.Collections.Generic;
using DG.Tweening;
using Match2.Core.Cube;
using Match2.Core.Grid;
using Match2.Core.Interfaces;
using Match2.Core.Services.Animation;
namespace Match2.Core.Services.Gravity
{
    public class GravityProcessor
    {
        private readonly Cell[,] grid;
        private readonly GridController gridController;
        private readonly AnimationController animationController;
        private readonly GravityChecker gravityChecker;
        private readonly int width;
        private readonly int height;

        public GravityProcessor(GridController gridController, AnimationController animationController)
        {
            this.grid = gridController.Grid;
            this.gridController = gridController;
            this.animationController = animationController;

            this.gravityChecker = new GravityChecker(grid);

            width = grid.GetLength(0);
            height = grid.GetLength(1);
        }

        public void StartGravitiyProcess()
        {
            var gravitySequence = DOTween.Sequence();

            for (int x = 0; x < width; x++)
            {
                var columnTween = ApplyGravityToColumn(x);
                if (columnTween != null)
                {
                    gravitySequence.Join(columnTween);
                }
            }

            gravitySequence.AppendCallback(FillEmptySpaces);
        }

        private void FillEmptySpaces()
        {
            var newCubes = new List<BaseCube>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = grid[x, y];

                    if (!gravityChecker.CanFillCell(cell, x, y))
                        continue;

                    var newCube = gridController.CubePlacementHandler.CreateRandomCube(cell, false);
                    if (newCube == null)
                        continue;

                    if (newCube is IMoveable moveable)
                    {
                        moveable.OnMoveStarted();
                        newCubes.Add(newCube);
                    }
                }
            }

            if (newCubes.Count > 0)
            {
                animationController.CreateSpawnSequence(newCubes, () =>
                {
                    CompleteCubeMovements(newCubes);
                    gridController.CheckAndShuffleIfNeeded();
                });
            }
            else
            {
                gridController.CheckAndShuffleIfNeeded();
            }
        }

        private Tween ApplyGravityToColumn(int column)
        {
            var fallingCubes = new List<BaseCube>();

            for (int row = 0; row < height; row++)
            {
                var currentCell = grid[column, row];

                if (currentCell.IsDeadCell || currentCell.CurrentCube != null)
                    continue;

                var fallingCube = gravityChecker.FindMovableCubeAbove(column, row);
                if (fallingCube == null)
                    continue;

                var moveable = fallingCube as IMoveable;
                if (!moveable?.CanMove() ?? true) continue;


                MoveCubeToNewPosition(fallingCube, currentCell);
                moveable.OnMoveStarted();
                fallingCubes.Add(fallingCube);
            }

            if (fallingCubes.Count == 0)
                return null;

            var dropSequence = animationController.CreateDropSequence(fallingCubes);
            dropSequence.OnComplete(() => CompleteCubeMovements(fallingCubes));

            return dropSequence;
        }

        private void MoveCubeToNewPosition(BaseCube cube, Cell targetCell)
        {
            var sourceCell = cube.CurrentCell;
            sourceCell.SetCube(null);
            targetCell.SetCube(cube);
        }

        private void CompleteCubeMovements(List<BaseCube> cubes)
        {
            foreach (var cube in cubes)
            {
                if (cube is not IMoveable moveable) return;
                moveable.OnMoveCompleted();
            }
        }
    }
}