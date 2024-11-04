using UnityEngine;

public class LevelEditorSettings
{
    public float cellSize = 40f;
    public float padding = 2f;

    public Color gridBackgroundColor = new Color(0.2f, 0.2f, 0.2f);
    public Color gridLineColor = new Color(0.3f, 0.3f, 0.3f);
    public Color deadCellColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
    public Color obstacleCellColor = new Color(0.8f, 0.4f, 0.1f, 0.8f);
    public Color cellBackgroundColor = new Color(1f, 1f, 1f, 0.1f);
}