using Match2.Core.Interfaces;
using TMPro;
using UnityEngine;

namespace Match2.Core.Cube
{
    public class ObstacleCube : BaseCube, IDamageable
    {
        [SerializeField] private TextMeshProUGUI healthText;
        private int currentHealth;

        public override void Initialize(int health)
        {
            UpdateHealthDisplay();
            base.Initialize(health);
        }

        public bool CanTakeDamage() => currentHealth > 0;
        public void TakeDamage(int damage)
        {
            if (!CanTakeDamage()) return;

            currentHealth = Mathf.Max(0, currentHealth - damage);
            UpdateHealthDisplay();
        }
        public void SetHealth(int health)
        {
            currentHealth = health;
            UpdateHealthDisplay();
        }
        public int GetHealth() => currentHealth;

        private void UpdateHealthDisplay()
        {
            healthText.text = currentHealth.ToString();
        }


    }
}