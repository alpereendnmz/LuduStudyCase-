namespace Match2.Core.Interfaces
{
    public interface IDamageable
    {
        bool CanTakeDamage();
        void TakeDamage(int damage);
        void SetHealth(int health);
        int GetHealth();
    }
}