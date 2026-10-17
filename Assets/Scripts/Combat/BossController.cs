using UnityEngine;

namespace VoxelPrototype
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 500f;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float meleeRange = 4f;
        [SerializeField] private float projectileRange = 12f;
        [SerializeField] private float meleeDamage = 15f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private Transform target;
        [SerializeField] private GameObject projectilePrefab;

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

            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;

            if (distance > meleeRange && distance < projectileRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }

            if (distance <= meleeRange && Time.time >= nextAttackTime)
            {
                MeleeAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else if (distance <= projectileRange && Time.time >= nextAttackTime)
            {
                RangedAttack();
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

        private void MeleeAttack()
        {
            var survival = target.GetComponent<SurvivalSystem>();
            if (survival != null)
            {
                survival.ApplyDamage(meleeDamage);
            }
        }

        private void RangedAttack()
        {
            if (projectilePrefab == null)
            {
                MeleeAttack();
                return;
            }

            var projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            var projectileBody = projectile.GetComponent<Rigidbody>();
            var direction = (target.position - transform.position).normalized;

            if (projectileBody != null)
            {
                projectileBody.AddForce(direction * 700f, ForceMode.Impulse);
            }

            var damageScript = projectile.GetComponent<ProjectileDamage>();
            if (damageScript != null)
            {
                damageScript.SetDamage(meleeDamage * 0.8f);
            }
        }

        private void DefeatBoss()
        {
            Destroy(gameObject);
        }
    }
}
