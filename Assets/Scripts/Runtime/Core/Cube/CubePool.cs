using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;
using Match2.Core.Data;
using Match2.Core.Grid;
using Match2.Core.Types;

namespace Match2.Core.Cube
{
    public class CubePool : MonoBehaviour
    {
        [SerializeField] private GridController gridController;
        [SerializeField] private Transform poolContainer;

        private Dictionary<ColorType, ObjectPool<ColorCube>> colorCubePools;
        private Dictionary<PowerUpType, ObjectPool<PowerUpCube>> powerUpPools;
        private ObjectPool<ObstacleCube> obstaclePool;

        private CubeDatabase cubeDatabase;
        private const int DefaultCapacity = 50;
        private const int MaxCapacity = 200;

        public void Initialize(CubeDatabase database)
        {
            cubeDatabase = database;

            InitializePools();
        }

        private void InitializePools()
        {
            colorCubePools = new Dictionary<ColorType, ObjectPool<ColorCube>>();
            powerUpPools = new Dictionary<PowerUpType, ObjectPool<PowerUpCube>>();

            foreach (ColorType color in System.Enum.GetValues(typeof(ColorType)))
            {
                var colorData = cubeDatabase.GetColorCube(color);
                if (colorData != null)
                {
                    colorCubePools[color] = CreateColorCubePool(colorData);
                }
            }

            foreach (PowerUpType powerUpType in System.Enum.GetValues(typeof(PowerUpType)))
            {
                if (powerUpType != PowerUpType.None)
                {
                    var powerUpData = cubeDatabase.GetPowerUpCube(powerUpType);
                    if (powerUpData != null)
                    {
                        powerUpPools[powerUpType] = CreatePowerUpPool(powerUpData);
                    }
                }
            }

            var obstacleData = cubeDatabase.GetObstacleCube();
            if (obstacleData != null)
            {
                obstaclePool = CreateObstaclePool(obstacleData);
            }
        }

        private ObjectPool<ColorCube> CreateColorCubePool(ColorCubeData cubeData)
        {
            return new ObjectPool<ColorCube>(
                createFunc: () => CreateColorCube(cubeData),
                actionOnGet: cube => OnCubeRetrieved(cube),
                actionOnRelease: cube => OnCubeReleased(cube),
                actionOnDestroy: cube => Destroy(cube.gameObject),
                defaultCapacity: DefaultCapacity,
                maxSize: MaxCapacity
            );
        }

        private ObjectPool<PowerUpCube> CreatePowerUpPool(PowerUpCubeData powerUpData)
        {
            return new ObjectPool<PowerUpCube>(
                createFunc: () => CreatePowerUpCube(powerUpData, gridController),
                actionOnGet: cube => OnCubeRetrieved(cube),
                actionOnRelease: cube => OnCubeReleased(cube),
                actionOnDestroy: cube => Destroy(cube.gameObject),
                defaultCapacity: DefaultCapacity,
                maxSize: MaxCapacity
            );
        }

        private ObjectPool<ObstacleCube> CreateObstaclePool(ObstacleCubeData obstacleData)
        {
            return new ObjectPool<ObstacleCube>(
                createFunc: () => CreateObstacleCube(obstacleData),
                actionOnGet: cube => OnCubeRetrieved(cube),
                actionOnRelease: cube => OnCubeReleased(cube),
                actionOnDestroy: cube => Destroy(cube.gameObject),
                defaultCapacity: DefaultCapacity,
                maxSize: MaxCapacity
            );
        }

        private ColorCube CreateColorCube(ColorCubeData cubeData)
        {
            var cube = Instantiate(cubeData.prefab, poolContainer).GetComponent<ColorCube>();
            cube.Initialize(cubeData.colorType);
            return cube;
        }

        private PowerUpCube CreatePowerUpCube(PowerUpCubeData powerUpData, GridController gridController)
        {
            var cube = Instantiate(powerUpData.prefab, poolContainer).GetComponent<PowerUpCube>();
            cube.Initialize(gridController, powerUpData.powerUpType);
            return cube;
        }

        private ObstacleCube CreateObstacleCube(ObstacleCubeData obstacleData)
        {
            var cube = Instantiate(obstacleData.prefab, poolContainer).GetComponent<ObstacleCube>();
            cube.Initialize(obstacleData.maxHealth);

            return cube;
        }

        private void OnCubeRetrieved(BaseCube cube)
        {
            cube.gameObject.SetActive(true);
        }

        private void OnCubeReleased(BaseCube cube)
        {
            cube.gameObject.SetActive(false);
            cube.transform.SetParent(poolContainer);
        }

        public ColorCube GetColorCube(ColorType colorType)
        {
            if (colorCubePools.TryGetValue(colorType, out var pool))
            {
                return pool.Get();
            }
            return null;
        }

        public PowerUpCube GetPowerUpCube(PowerUpType powerUpType)
        {
            if (powerUpPools.TryGetValue(powerUpType, out var pool))
            {
                return pool.Get();
            }
            return null;
        }

        public ObstacleCube GetObstacleCube(int health)
        {
            var cube = obstaclePool.Get();
            cube.SetHealth(health);
            return cube;
        }

        public void ReleaseCube(BaseCube cube)
        {
            if (cube == null) return;

            if (cube is ColorCube colorCube)
            {
                colorCubePools[colorCube.GetColorType()].Release(colorCube);
            }
            else if (cube is PowerUpCube powerUpCube)
            {
                powerUpPools[powerUpCube.GetPowerUpType()].Release(powerUpCube);
            }
            else if (cube is ObstacleCube obstacleCube)
            {
                obstaclePool.Release(obstacleCube);
            }
        }
    }
}