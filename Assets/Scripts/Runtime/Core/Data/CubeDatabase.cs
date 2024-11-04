using System.Collections.Generic;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Data
{
    [CreateAssetMenu(fileName = "CubeDatabase", menuName = "Match2/Cube Database")]
    public class CubeDatabase : ScriptableObject
    {
        [Header("Color Cubes")]
        [SerializeField] private List<ColorCubeData> colorCubes;

        [Header("Obstacles")]
        [SerializeField] private ObstacleCubeData obstacleCube;

        [Header("PowerUps")]
        [SerializeField] private List<PowerUpCubeData> powerUpCubes;

        private Dictionary<ColorType, ColorCubeData> colorCubeMap;

        private void OnEnable()
        {
            InitializeColorMap();
        }

        private void InitializeColorMap()
        {
            colorCubeMap = new Dictionary<ColorType, ColorCubeData>();
            foreach (var cubeData in colorCubes)
            {
                colorCubeMap[cubeData.colorType] = cubeData;
            }
        }

        public ColorCubeData GetRandomColorCube(int availableColors)
        {
            int randomIndex = Random.Range(0, Mathf.Min(availableColors, colorCubes.Count));
            return colorCubes[randomIndex];
        }

        public ColorCubeData GetColorCube(ColorType colorType)
        {
            return colorCubeMap.TryGetValue(colorType, out var cubeData) ? cubeData : null;
        }

        public ObstacleCubeData GetObstacleCube()
        {
            return obstacleCube;
        }

        public PowerUpCubeData GetPowerUpCube(PowerUpType powerUpType)
        {
            return powerUpCubes.Find(x => x.powerUpType == powerUpType);
        }
    }
    [System.Serializable]
    public class BaseCubeData
    {
        public GameObject prefab;
    }

    [System.Serializable]
    public class ColorCubeData : BaseCubeData
    {
        public ColorType colorType;
        public Color color = Color.white;
    }

    [System.Serializable]
    public class ObstacleCubeData : BaseCubeData
    {
        public int maxHealth;
    }

    [System.Serializable]
    public class PowerUpCubeData : BaseCubeData
    {
        public PowerUpType powerUpType;
    }
}
