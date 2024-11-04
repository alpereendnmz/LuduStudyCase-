// LevelEditorGrid.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Match2.Core.Data;

public class LevelEditorGrid
{
    private readonly LevelEditorSettings settings;
    private Rect gridRect;
    private LevelData currentLevel;

    public Rect GridRect => gridRect;

    public LevelEditorGrid(LevelEditorSettings settings)
    {
        this.settings = settings;
    }

    public void SetCurrentLevel(LevelData level)
    {
        currentLevel = level;
    }

    public void Draw(Dictionary<Vector2Int, ObstacleData> obstaclePositions)
    {
        if (currentLevel == null) return;

        float totalWidth = CalculateTotalGridWidth();
        float totalHeight = CalculateTotalGridHeight();

        gridRect = GUILayoutUtility.GetRect(totalWidth, totalHeight);
        EditorGUI.DrawRect(gridRect, settings.gridBackgroundColor);

        // Y eksenini ters çevirerek çizim yapıyoruz
        for (int x = 0; x < currentLevel.width; x++)
        {
            for (int y = currentLevel.height - 1; y >= 0; y--)
            {
                DrawCell(x, y, obstaclePositions);
            }
        }
    }



    private float CalculateTotalGridWidth()
    {
        return (currentLevel.width * currentLevel.cellSize) +
               ((currentLevel.width + 1) * currentLevel.cellSpacing);
    }

    private float CalculateTotalGridHeight()
    {
        return (currentLevel.height * currentLevel.cellSize) +
               ((currentLevel.height + 1) * currentLevel.cellSpacing);
    }

    private void DrawCell(int x, int y, Dictionary<Vector2Int, ObstacleData> obstaclePositions)
    {
        Vector2Int pos = new Vector2Int(x, y);
        Rect cellRect = GetCellRect(x, y);

        // Hücre içeriğini çiz
        EditorGUI.DrawRect(cellRect, settings.cellBackgroundColor);

        // Dead cell veya obstacle kontrolü
        if (currentLevel.deadCells.Contains(pos))
        {
            EditorGUI.DrawRect(cellRect, settings.deadCellColor);
            // Visual debug için çerçeve çiz
            DrawDebugFrame(cellRect, Color.red);
        }
        else if (obstaclePositions.TryGetValue(pos, out ObstacleData obstacle))
        {
            EditorGUI.DrawRect(cellRect, settings.obstacleCellColor);
            DrawObstacleHealth(cellRect, obstacle.health);
            // Visual debug için çerçeve çiz
            DrawDebugFrame(cellRect, Color.yellow);
        }

        DrawCellBorder(cellRect);
    }
    private void DrawDebugFrame(Rect rect, Color color)
    {
#if UNITY_EDITOR
        if (Debug.isDebugBuild)
        {
            var prevColor = Handles.color;
            Handles.color = color;
            Handles.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0),
                               new Vector3(rect.width, rect.height, 0));
            Handles.color = prevColor;
        }
#endif
    }


    private void DrawObstacleHealth(Rect cellRect, int health)
    {
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
        GUI.Label(cellRect, health.ToString(), style);
    }

    private void DrawCellBorder(Rect cellRect)
    {
        Handles.color = settings.gridLineColor;
        Handles.DrawPolyLine(
            new Vector3(cellRect.xMin, cellRect.yMin),
            new Vector3(cellRect.xMax, cellRect.yMin),
            new Vector3(cellRect.xMax, cellRect.yMax),
            new Vector3(cellRect.xMin, cellRect.yMax),
            new Vector3(cellRect.xMin, cellRect.yMin)
        );
    }

    public Rect GetCellRect(int x, int y)
    {
        float startX = gridRect.x + currentLevel.cellSpacing;
        float startY = gridRect.y + currentLevel.cellSpacing;

        // Y pozisyonunu ters çevir
        int invertedY = (currentLevel.height - 1) - y;

        float xPos = startX + (x * (currentLevel.cellSize + currentLevel.cellSpacing));
        float yPos = startY + (invertedY * (currentLevel.cellSize + currentLevel.cellSpacing));

        return new Rect(xPos, yPos, currentLevel.cellSize, currentLevel.cellSize);
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        // Mouse pozisyonunu grid'e göre ayarla
        Vector2 relativePos = mousePosition - gridRect.position -
                             new Vector2(currentLevel.cellSpacing, currentLevel.cellSpacing);

        int x = Mathf.FloorToInt(relativePos.x / (currentLevel.cellSize + currentLevel.cellSpacing));
        int y = Mathf.FloorToInt(relativePos.y / (currentLevel.cellSize + currentLevel.cellSpacing));

        // Mouse pozisyonunu grid koordinatlarına çevir
        y = currentLevel.height - 1 - y;

        x = Mathf.Clamp(x, 0, currentLevel.width - 1);
        y = Mathf.Clamp(y, 0, currentLevel.height - 1);

        return new Vector2Int(x, y);
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < currentLevel.width &&
               pos.y >= 0 && pos.y < currentLevel.height;
    }

}