using System.Collections.Generic;
using Match2.Core.Grid;
using Match2.Core.Interfaces;
using Match2.Core.Types;

namespace Match2.Core.Cube
{
    public abstract class PowerUpCube : BaseCube, IMoveable, IPowerUpActivatable
    {
        private PowerUpType powerUpType;
        public bool IsMoving { get; set; }
        public PowerUpType GetPowerUpType() => powerUpType;

        public bool CanMove() => true;
        public void OnMoveStarted() => IsMoving = true;
        public void OnMoveCompleted() => IsMoving = false;
        public void SetMovementProgress(float progress) { }

        public abstract List<Cell> GetAffectedCells();
        public override void Initialize(GridController gridController, PowerUpType powerUpType)
        {
            base.Initialize(gridController, powerUpType);
            this.powerUpType = powerUpType;
        }


    }
}