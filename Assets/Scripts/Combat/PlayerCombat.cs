using UnityEngine;

namespace VoxelPrototype
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float attackDamage = 25f;
        [SerializeField] private float attackCooldown = 0.75f;
        [SerializeField] private Transform attackOrigin;

        private float nextAttackTime;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
            {
                TryAttack();
            }
        }

        public void TryAttack()
        {
            nextAttackTime = Time.time + attackCooldown;

            Vector3 start = attackOrigin != null ? attackOrigin.position : transform.position + Vector3.up * 1.5f;
            Collider[] hits = Physics.OverlapSphere(start, attackRange);

            foreach (Collider hit in hits)
            {
                if (hit.transform == transform)
                {
                    continue;
                }

                var boss = hit.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(attackDamage);
                    continue;
                }

                var resourceNode = hit.GetComponent<ResourceNode>();
                if (resourceNode != null)
                {
                    resourceNode.Harvest();
                }
            }
        }
    }
}
