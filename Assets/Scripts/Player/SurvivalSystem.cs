using UnityEngine;

namespace VoxelPrototype
{
    public class SurvivalSystem : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxHunger = 100f;
        [SerializeField] private float healthDrainRate = 1.2f;
        [SerializeField] private float hungerDrainRate = 0.6f;

        private float currentHealth;
        private float currentHunger;

        public float CurrentHealth => currentHealth;
        public float CurrentHunger => currentHunger;

        private void Start()
        {
            currentHealth = maxHealth;
            currentHunger = maxHunger;
        }

        private void Update()
        {
            currentHunger = Mathf.Max(0f, currentHunger - hungerDrainRate * Time.deltaTime);

            if (currentHunger <= 0f)
            {
                currentHealth = Mathf.Max(0f, currentHealth - healthDrainRate * Time.deltaTime);
            }

            if (currentHealth <= 0f)
            {
                Respawn();
            }
        }

        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
        }

        public void EatFood(float hungerRestore)
        {
            currentHunger = Mathf.Min(maxHunger, currentHunger + hungerRestore);
            currentHealth = Mathf.Min(maxHealth, currentHealth + 8f);
        }

        private void Respawn()
        {
            currentHealth = maxHealth;
            currentHunger = maxHunger;
            transform.position = Vector3.zero;
        }
    }
}
