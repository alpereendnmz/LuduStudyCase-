namespace Match2.Core.Interfaces
{
    public interface IMoveable
    {
        bool IsMoving { get; set; }
        bool CanMove();
        void OnMoveStarted();
        void OnMoveCompleted();
        void SetMovementProgress(float progress);
    }
}