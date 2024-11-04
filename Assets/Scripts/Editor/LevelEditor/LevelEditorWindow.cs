using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Match2.Core.Data;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevel;
    private LevelEditorSettings settings;
    private LevelEditorGrid grid;
    private LevelEditorInputHandler inputHandler;
    private LevelEditorUI ui;
    private Vector2 inspectorScrollPosition;
    private Vector2 gridScrollPosition;

    [MenuItem("Tools/Match-3/Level Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>("Level Editor");
        window.minSize = new Vector2(800, 500);
    }

    private void OnEnable()
    {
        settings = new LevelEditorSettings();
        grid = new LevelEditorGrid(settings);
        inputHandler = new LevelEditorInputHandler(grid);
        ui = new LevelEditorUI(inputHandler);

        if (currentLevel != null)
        {
            OnLevelLoaded();
        }
    }

    private void OnGUI()
    {
        ui.DrawToolbar(CreateNewLevel, LoadLevel, SaveLevel, currentLevel?.name);

        if (currentLevel == null)
        {
            EditorGUILayout.HelpBox("Please select or create a Level Data asset.", MessageType.Info);
            return;
        }

        EditorGUILayout.BeginHorizontal();

        DrawGridPanel();
        DrawInspectorPanel();

        EditorGUILayout.EndHorizontal();

        ProcessEvents();

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawGridPanel()
    {
        float gridPanelWidth = position.width * 0.7f;
        EditorGUILayout.BeginVertical(GUILayout.Width(gridPanelWidth));

        gridScrollPosition = EditorGUILayout.BeginScrollView(gridScrollPosition);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        grid.Draw(inputHandler.GetObstaclePositions());

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        Rect separatorRect = EditorGUILayout.GetControlRect(false, position.height, GUILayout.Width(1));
        EditorGUI.DrawRect(separatorRect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void DrawInspectorPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(300));

        inspectorScrollPosition = EditorGUILayout.BeginScrollView(inspectorScrollPosition);

        EditorGUILayout.Space(10);
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField("Level Settings", headerStyle);
        EditorGUILayout.Space(10);

        ui.DrawLevelSettings(currentLevel);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Tools", headerStyle);
        EditorGUILayout.Space(10);

        ui.DrawGridTools();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void ProcessEvents()
    {
        Event e = Event.current;
        inputHandler.ProcessInput(e);
    }

    private void CreateNewLevel()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Create New Level",
            "NewLevel",
            "asset",
            "Create a new level asset"
        );

        if (!string.IsNullOrEmpty(path))
        {
            LevelData newLevel = CreateInstance<LevelData>();
            InitializeDefaultLevelSettings(newLevel);

            AssetDatabase.CreateAsset(newLevel, path);
            AssetDatabase.SaveAssets();

            currentLevel = newLevel;
            OnLevelLoaded();
        }
    }

    private void LoadLevel()
    {
        string path = EditorUtility.OpenFilePanel("Select Level Data", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
            LevelData loadedLevel = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            if (loadedLevel != null)
            {
                currentLevel = loadedLevel;
                OnLevelLoaded();
            }
        }
    }

    private void SaveLevel()
    {
        if (currentLevel != null)
        {
            EditorUtility.SetDirty(currentLevel);
            AssetDatabase.SaveAssets();
            Debug.Log($"Level saved: {AssetDatabase.GetAssetPath(currentLevel)}");
        }
    }

    private void OnLevelLoaded()
    {
        grid.SetCurrentLevel(currentLevel);
        inputHandler.SetCurrentLevel(currentLevel);
        Repaint();
    }

    private void InitializeDefaultLevelSettings(LevelData level)
    {
        level.width = 8;
        level.height = 8;
        level.cellSize = 100f;
        level.cellSpacing = 10f;
        level.colorCount = 5;
        level.minMatchCount = 3;
        level.targetMatchCount = 15;
        level.totalMoves = 30;
        level.timeLimit = 60f;
        level.rocketMatchCount = 4;
        level.bombMatchCount = 5;
        level.deadCells = new List<Vector2Int>();
        level.initialObstacles = new List<ObstacleData>();
    }
}