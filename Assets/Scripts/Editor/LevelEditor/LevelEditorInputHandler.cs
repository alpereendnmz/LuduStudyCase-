// LevelEditorInputHandler.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Match2.Core.Data;

public class LevelEditorInputHandler
{
    private readonly LevelEditorGrid grid;
    private LevelData currentLevel;
    private Dictionary<Vector2Int, ObstacleData> obstaclePositions;

    public bool IsPlacingObstacles { get; set; }
    public bool IsErasingCells { get; set; }
    public int SelectedObstacleHealth { get; set; } = 1;

    public LevelEditorInputHandler(LevelEditorGrid grid)
    {
        this.grid = grid;
        obstaclePositions = new Dictionary<Vector2Int, ObstacleData>();
    }

    public void SetCurrentLevel(LevelData level)
    {
        currentLevel = level;
        LoadObstaclePositions();
    }

    public void ProcessInput(Event e)
    {
        if (currentLevel == null) return;

        switch (e.type)
        {
            case EventType.MouseDown:
            case EventType.MouseDrag:
                HandleMouseEvent(e);
                break;
        }
    }
    private void HandleMouseEvent(Event e)
    {
        Vector2 mousePosition = e.mousePosition;

        // Grid içinde mi kontrol et
        if (grid.GridRect.Contains(mousePosition))
        {
            Vector2Int gridPosition = grid.GetGridPosition(mousePosition);

            if (grid.IsValidPosition(gridPosition))
            {
                HandleMouseInput(e.button, gridPosition);
                GUI.changed = true;
                e.Use();
            }
        }
    }
    private void HandleMouseInput(int button, Vector2Int position)
    {
        if (button == 0)
        {
            if (IsErasingCells)
            {
                AddDeadCell(position);
            }
            else if (IsPlacingObstacles)
            {
                AddObstacle(position);
            }
        }
        else if (button == 1)
        {
            ClearCell(position);
        }
    }

    private void AddDeadCell(Vector2Int position)
    {
        if (!currentLevel.deadCells.Contains(position))
        {
            currentLevel.deadCells.Add(position);
            obstaclePositions.Remove(position);
            UpdateObstacleList();
            GUI.changed = true;
        }
    }

    private void AddObstacle(Vector2Int position)
    {
        if (!currentLevel.deadCells.Contains(position))
        {
            var obstacle = new ObstacleData
            {
                position = position,
                health = SelectedObstacleHealth
            };
            obstaclePositions[position] = obstacle;
            UpdateObstacleList();
            GUI.changed = true;
        }
    }

    private void ClearCell(Vector2Int position)
    {
        bool changed = currentLevel.deadCells.Remove(position) ||
                      obstaclePositions.Remove(position);
        if (changed)
        {
            UpdateObstacleList();
            GUI.changed = true;
        }
    }

    private void LoadObstaclePositions()
    {
        obstaclePositions.Clear();
        if (currentLevel.initialObstacles != null)
        {
            foreach (var obstacle in currentLevel.initialObstacles)
            {
                obstaclePositions[obstacle.position] = obstacle;
            }
        }
    }

    private void UpdateObstacleList()
    {
        currentLevel.initialObstacles.Clear();
        currentLevel.initialObstacles.AddRange(obstaclePositions.Values);
        EditorUtility.SetDirty(currentLevel);
    }

    public Dictionary<Vector2Int, ObstacleData> GetObstaclePositions() => obstaclePositions;
}