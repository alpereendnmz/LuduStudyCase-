using UnityEngine;

namespace Match2.Core.Services.LevelLogic
{
    public class MoveCounter
    {
        private int remainingMoves;

        public int RemainingMoves => remainingMoves;

        public void Initialize(int totalMoves)
        {
            remainingMoves = totalMoves;
        }

        public void DecreaseMoves()
        {
            remainingMoves = Mathf.Max(0, remainingMoves - 1);
        }

        public bool IsMovesLeft() => remainingMoves > 0;
    }
}