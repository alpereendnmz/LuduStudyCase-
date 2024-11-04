using System.Collections.Generic;
using Match2.Core.Grid;
using Match2.Core.Types;

namespace Match2.Core.Interfaces
{
    public interface IPowerUpActivatable
    {
        PowerUpType GetPowerUpType();
        abstract List<Cell> GetAffectedCells();

    }
}
