using Match2.Core.Types;

namespace Match2.Core.Interfaces
{
    public interface IMatchable
    {
        ColorType GetColorType();
        void OnMatched();
        bool CanBeMatched();
    }
}