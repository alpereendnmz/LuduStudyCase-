using System;
using Match2.Core.Cube;
using Match2.Core.Grid;
using Match2.Core.Services.Animation;
using Match2.Core.Services.Gravity;
using Match2.Core.Util;
namespace Match2.Core.Services.Matching
{
    public class MatchProcessor
    {
        public event Action OnMatchProcessed;

        private readonly MatchFinder matchFinder;
        private readonly NormalMatchHandler normalMatchHandler;
        private readonly PowerUpMatchHandler powerUpMatchHandler;

        public MatchProcessor(MatchFinder matchFinder, int minMatchCount, PowerupManager powerupManager, GravityProcessor gravityProcessor, AnimationController animationController)
        {
            this.matchFinder = matchFinder;
            this.normalMatchHandler = new NormalMatchHandler(minMatchCount, powerupManager, gravityProcessor, animationController);
            this.powerUpMatchHandler = new PowerUpMatchHandler(gravityProcessor, animationController);

            CellEvents.OnCellClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cell)
        {
            var cube = cell.CurrentCube;
            if (cube == null) return;

            if (cube is PowerUpCube powerUp)
            {
                var matches = powerUp.GetAffectedCells();
                powerUpMatchHandler.Process(matches, cell, OnMatchProcessed);
            }
            else
            {
                var matches = matchFinder.FindMatches(cell);
                normalMatchHandler.Process(matches, cell, OnMatchProcessed);
            }
        }
    }
}