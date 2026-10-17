using UnityEngine;

namespace VoxelPrototype
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 500f;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private float meleeDamage = 15f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private Transform target;

        private float currentHealth;
        private float nextAttackTime;

        private void Start()
        {
            currentHealth = maxHealth;
            if (target == null)
            {
                target = FindObjectOfType<PlayerController>()?.transform;
            }
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            Vector3 direction = (target.position - transform.position);
            float distance = direction.magnitude;

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
            else if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                DefeatBoss();
            }
        }

        private void Attack()
        {
            var survival = target.GetComponent<SurvivalSystem>();
            if (survival != null)
            {
                survival.ApplyDamage(meleeDamage);
            }
        }

        private void DefeatBoss()
        {
            Destroy(gameObject);
        }
    }
}
