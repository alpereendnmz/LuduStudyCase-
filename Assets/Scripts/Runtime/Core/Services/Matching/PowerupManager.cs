using Match2.Core.Cube;
using Match2.Core.Data;
using Match2.Core.Grid;
using Match2.Core.Types;
using UnityEngine;
namespace Match2.Core.Services.Matching
{
    public class PowerupManager
    {
        private readonly LevelData levelData;
        private readonly CubeFactory cubeFactory;
        public PowerupManager(LevelData levelData, CubeFactory cubeFactory)
        {
            this.levelData = levelData;
            this.cubeFactory = cubeFactory;
        }
        public void CheckForPowerUP(int matchCount, Cell originCell, BaseCube cube)
        {
            if (ShouldCreatePowerUp(matchCount, cube)) CreatePowerUp(matchCount, originCell);
        }

        private bool ShouldCreatePowerUp(int matchCount, BaseCube cube)
        {
            if (cube is PowerUpCube)
                return false;

            return matchCount >= levelData.rocketMatchCount || matchCount >= levelData.bombMatchCount;
        }

        private void CreatePowerUp(int matchCount, Cell originCell)
        {
            PowerUpType powerUpType;

            if (matchCount >= levelData.bombMatchCount)
            {
                powerUpType = PowerUpType.Bomb;
            }
            else if (matchCount >= levelData.rocketMatchCount)
            {
                powerUpType = Random.Range(0, 2) == 0
                    ? PowerUpType.HorizontalRocket
                    : PowerUpType.VerticalRocket;
            }
            else return;

            var powerUp = cubeFactory.CreatePowerUp(powerUpType);
            originCell.SetCube(powerUp, true);
        }
    }
}