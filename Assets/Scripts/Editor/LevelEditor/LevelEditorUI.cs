using UnityEngine;
using UnityEditor;
using Match2.Core.Data;


public class LevelEditorUI
{
    private readonly LevelEditorInputHandler inputHandler;
    private SerializedObject serializedLevel;
    private GUIStyle sectionHeaderStyle;

    public LevelEditorUI(LevelEditorInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        InitializeStyles();
    }

    private void InitializeStyles()
    {
        sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(5, 5, 5, 5),
            fontSize = 12
        };
    }

    public void DrawToolbar(System.Action onNew, System.Action onLoad, System.Action onSave, string currentLevelName)
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            if (GUILayout.Button("New Level", EditorStyles.toolbarButton))
                onNew?.Invoke();

            if (GUILayout.Button("Load Level", EditorStyles.toolbarButton))
                onLoad?.Invoke();

            if (GUILayout.Button("Save Level", EditorStyles.toolbarButton))
                onSave?.Invoke();

            GUILayout.FlexibleSpace();

            if (!string.IsNullOrEmpty(currentLevelName))
            {
                GUILayout.Label($"Current: {currentLevelName}", EditorStyles.toolbarButton);
            }
        }
    }

    public void DrawLevelSettings(LevelData currentLevel)
    {
        if (currentLevel == null) return;

        serializedLevel = new SerializedObject(currentLevel);

        DrawSettingsSection("Grid Settings", () =>
        {
            currentLevel.width = EditorGUILayout.IntSlider("Width", currentLevel.width, 4, 12);
            currentLevel.height = EditorGUILayout.IntSlider("Height", currentLevel.height, 4, 12);
            currentLevel.cellSize = EditorGUILayout.Slider("Cell Size", currentLevel.cellSize, 40f, 120f);
            currentLevel.cellSpacing = EditorGUILayout.Slider("Cell Spacing", currentLevel.cellSpacing, 0f, 20f);
        });

        DrawSettingsSection("Game Rules", () =>
        {
            currentLevel.colorCount = EditorGUILayout.IntSlider("Colors", currentLevel.colorCount, 3, 6);
            currentLevel.minMatchCount = EditorGUILayout.IntField("Min Match", currentLevel.minMatchCount);
            currentLevel.targetMatchCount = EditorGUILayout.IntField("Target Match", currentLevel.targetMatchCount);
            currentLevel.totalMoves = EditorGUILayout.IntField("Total Moves", currentLevel.totalMoves);
            currentLevel.timeLimit = EditorGUILayout.FloatField("Time Limit", currentLevel.timeLimit);
        });

        DrawSettingsSection("Power-Ups", () =>
        {
            currentLevel.rocketMatchCount = EditorGUILayout.IntField("Rocket Match", currentLevel.rocketMatchCount);
            currentLevel.bombMatchCount = EditorGUILayout.IntField("Bomb Match", currentLevel.bombMatchCount);
        });

        if (serializedLevel.hasModifiedProperties)
        {
            serializedLevel.ApplyModifiedProperties();
            EditorUtility.SetDirty(currentLevel);
        }
    }

    public void DrawGridTools()
    {
        DrawSettingsSection("Grid Tools", () =>
        {
            EditorGUILayout.BeginHorizontal();
            bool newIsErasingCells = GUILayout.Toggle(
                inputHandler.IsErasingCells,
                "Erase Cells",
                EditorStyles.miniButtonLeft
            );
            bool newIsPlacingObstacles = GUILayout.Toggle(
                inputHandler.IsPlacingObstacles,
                "Place Obstacles",
                EditorStyles.miniButtonRight
            );
            EditorGUILayout.EndHorizontal();

            HandleToolToggles(newIsErasingCells, newIsPlacingObstacles);

            if (inputHandler.IsPlacingObstacles)
            {
                EditorGUILayout.Space(5);
                inputHandler.SelectedObstacleHealth = EditorGUILayout.IntSlider(
                    "Obstacle Health",
                    inputHandler.SelectedObstacleHealth,
                    1,
                    5
                );
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox(
                "Left Click: Place/Erase\nRight Click: Clear Cell\nDrag: Multiple Cells",
                MessageType.Info
            );
        });
    }

    private void HandleToolToggles(bool newIsErasingCells, bool newIsPlacingObstacles)
    {
        if (newIsErasingCells != inputHandler.IsErasingCells)
        {
            inputHandler.IsErasingCells = newIsErasingCells;
            if (newIsErasingCells)
                inputHandler.IsPlacingObstacles = false;
        }

        if (newIsPlacingObstacles != inputHandler.IsPlacingObstacles)
        {
            inputHandler.IsPlacingObstacles = newIsPlacingObstacles;
            if (newIsPlacingObstacles)
                inputHandler.IsErasingCells = false;
        }
    }

    private void DrawSettingsSection(string title, System.Action drawContent)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField(title, sectionHeaderStyle);

        EditorGUI.indentLevel++;
        EditorGUILayout.Space(5);
        drawContent?.Invoke();
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }
}