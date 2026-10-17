using Mirror;
using UnityEngine;

namespace VoxelPrototype.Networking
{
    public class NetworkBossController : NetworkBehaviour
    {
        public float maxHealth = 500f;
        public float moveSpeed = 3.5f;
        public float meleeRange = 4f;
        public float projectileRange = 12f;
        public float meleeDamage = 15f;
        public float attackCooldown = 2f;
        public GameObject projectilePrefab;

        [SyncVar(hook = nameof(OnHealthChanged))]
        private float currentHealth;

        private float nextAttackTime;

        public override void OnStartServer()
        {
            base.OnStartServer();
            currentHealth = maxHealth;
        }

        [ServerCallback]
        private void Update()
        {
            // boss AI runs on server only
            if (!isServer) return;

            var players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0) return;

            Transform target = null;
            float bestDist = float.MaxValue;
            foreach (var p in players)
            {
                float d = Vector3.Distance(p.transform.position, transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    target = p.transform;
                }
            }

            if (target == null) return;

            if (bestDist > meleeRange && bestDist < projectileRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }

            if (bestDist <= meleeRange && Time.time >= nextAttackTime)
            {
                MeleeAttack(target);
                nextAttackTime = Time.time + attackCooldown;
            }
            else if (bestDist <= projectileRange && Time.time >= nextAttackTime)
            {
                RangedAttack(target);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        [Server]
        public void TakeDamageServer(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                RpcOnDefeated();
                NetworkServer.Destroy(gameObject);
            }
        }

        [ClientRpc]
        private void RpcOnDefeated()
        {
            // visual effects or reward logic for clients
        }

        private void MeleeAttack(Transform target)
        {
            var survival = target.GetComponent<NetworkSurvival>();
            if (survival != null)
            {
                survival.ApplyDamage(meleeDamage);
            }
        }

        private void RangedAttack(Transform target)
        {
            if (projectilePrefab == null)
            {
                MeleeAttack(target);
                return;
            }

            var proj = Instantiate(projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            var rb = proj.GetComponent<Rigidbody>();
            var dir = (target.position - transform.position).normalized;
            if (rb != null) rb.AddForce(dir * 700f, ForceMode.Impulse);

            var damageScript = proj.GetComponent<ProjectileDamage>();
            if (damageScript != null)
            {
                damageScript.SetDamage(meleeDamage * 0.8f);
            }

            NetworkServer.Spawn(proj);
        }

        private void OnHealthChanged(float oldVal, float newVal)
        {
            // client-side hook for health UI
        }
    }
}
