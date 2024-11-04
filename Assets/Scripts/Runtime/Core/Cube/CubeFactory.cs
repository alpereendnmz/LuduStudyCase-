using Match2.Core.Data;
using Match2.Core.Types;

namespace Match2.Core.Cube
{
    public class CubeFactory
    {
        private readonly CubeDatabase cubeDatabase;
        private readonly LevelData levelData;
        private readonly CubePool cubePool;

        public CubeFactory(CubeDatabase cubeDatabase, LevelData levelData, CubePool cubePool)
        {
            this.cubeDatabase = cubeDatabase;
            this.levelData = levelData;
            this.cubePool = cubePool;

            this.cubePool.Initialize(cubeDatabase);
        }

        public ColorCube CreateRandomColorCube()
        {
            ColorCubeData cubeData = cubeDatabase.GetRandomColorCube(levelData.colorCount);
            return cubePool.GetColorCube(cubeData.colorType);
        }

        public ObstacleCube CreateObstacle(int health)
        {
            return cubePool.GetObstacleCube(health);
        }

        public PowerUpCube CreatePowerUp(PowerUpType type)
        {
            return cubePool.GetPowerUpCube(type);
        }
    }
}