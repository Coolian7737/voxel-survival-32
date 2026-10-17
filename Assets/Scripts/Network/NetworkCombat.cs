using Mirror;
using UnityEngine;

namespace VoxelPrototype.Networking
{
    public class NetworkCombat : NetworkBehaviour
    {
        public float attackRange = 3.5f;
        public float attackDamage = 25f;
        public float attackCooldown = 0.75f;
        public Transform attackOrigin;

        private float nextAttackTime;

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
            {
                CmdPerformAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        [Command]
        private void CmdPerformAttack()
        {
            Vector3 start = attackOrigin != null ? attackOrigin.position : transform.position + Vector3.up * 1.5f;
            Collider[] hits = Physics.OverlapSphere(start, attackRange);

            foreach (var hit in hits)
            {
                if (hit.transform == transform) continue;

                var boss = hit.GetComponent<NetworkBossController>();
                if (boss != null)
                {
                    boss.TakeDamageServer(attackDamage);
                    continue;
                }

                var node = hit.GetComponent<ResourceNode>();
                if (node != null)
                {
                    // harvesting runs on server via the node's own logic
                    node.Harvest();
                }
            }
        }
    }
}
