using System;
using Match2.Core.Grid;
namespace Match2.Core.Util
{
    public static class CellEvents
    {
        public static event Action<Cell> OnCellClicked;
        public static void CellClicked(Cell cell) => OnCellClicked?.Invoke(cell);
    }
}