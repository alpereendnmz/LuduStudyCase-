using System.Collections.Generic;
using UnityEngine;
namespace Match2.Core.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Match2/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Grid Settings")]
        public int width = 8;
        public int height = 8;
        public float cellSize = 100f;
        public float cellSpacing = 10f;


        [Header("Game Rules")]
        [Range(3, 6)]
        public int colorCount = 5;
        [Min(2)]
        public int minMatchCount = 2;

        [Header("Win Condition")]
        public int targetMatchCount = 15;

        [Header("Game Limits")]
        [Min(1)]
        public int totalMoves = 30;
        public float timeLimit = 60f;

        [Header("PowerUp Settings")]
        public int rocketMatchCount = 4;
        public int bombMatchCount = 5;

        [Header("Grid Layout")]
        public List<Vector2Int> deadCells = new List<Vector2Int>();
        public List<ObstacleData> initialObstacles = new List<ObstacleData>();
    }

    [System.Serializable]
    public class ObstacleData
    {
        public Vector2Int position;
        [Range(1, 5)]
        public int health = 2;
    }
}
