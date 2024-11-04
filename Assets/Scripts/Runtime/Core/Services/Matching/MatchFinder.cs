using System.Collections.Generic;
using Match2.Core.Grid;
using Match2.Core.Interfaces;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Services.Matching
{
    public class MatchFinder
    {
        private readonly Cell[,] grid;
        private bool[,] visited;

        public MatchFinder(Cell[,] grid)
        {
            this.grid = grid;
            this.visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        }

        public List<Cell> FindMatches(Cell startCell)
        {
            ResetVisited();
            var matches = new List<Cell>();

            if (startCell == null || startCell.IsDeadCell || startCell.CurrentCube == null)
                return matches;

            var matchable = startCell.CurrentCube as IMatchable;
            if (matchable == null || !matchable.CanBeMatched())
                return matches;

            FloodFill(startCell, matchable.GetColorType(), matches);
            return matches;
        }

        private void FloodFill(Cell cell, ColorType targetColor, List<Cell> matches)
        {
            if (cell == null || cell.IsDeadCell || visited[cell.GridPosition.x, cell.GridPosition.y])
                return;

            var matchable = cell.CurrentCube as IMatchable;
            if (matchable == null || !matchable.CanBeMatched() || matchable.GetColorType() != targetColor)
                return;

            visited[cell.GridPosition.x, cell.GridPosition.y] = true;
            matches.Add(cell);

            Vector2Int[] directions = new[]
            {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

            foreach (var dir in directions)
            {
                var newPos = cell.GridPosition + dir;
                if (IsValidPosition(newPos))
                {
                    FloodFill(grid[newPos.x, newPos.y], targetColor, matches);
                }
            }
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < grid.GetLength(0) &&
                   pos.y >= 0 && pos.y < grid.GetLength(1);
        }

        private void ResetVisited()
        {
            visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        }
    }
}