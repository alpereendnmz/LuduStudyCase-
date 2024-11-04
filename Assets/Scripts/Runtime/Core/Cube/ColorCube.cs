
using Match2.Core.Cube;
using Match2.Core.Interfaces;
using Match2.Core.Types;

public class ColorCube : BaseCube, IMatchable, IMoveable
{
    private ColorType colorType;
    public bool IsMoving { get; set; }

    public override void Initialize(ColorType colorType)
    {
        this.colorType = colorType;
        base.Initialize(colorType);
    }

    public ColorType GetColorType() => colorType;
    public bool CanBeMatched() => !IsMoving;
    public void OnMatched() { }

    public bool CanMove() => true;
    public void OnMoveStarted() => IsMoving = true;
    public void OnMoveCompleted() => IsMoving = false;
    public void SetMovementProgress(float progress) { }
}
