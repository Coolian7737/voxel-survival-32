using Mirror;
using UnityEngine;

namespace VoxelPrototype.Networking
{
    public class NetworkSurvival : NetworkBehaviour
    {
        [Header("Survival")]
        public float maxHealth = 100f;
        public float maxHunger = 100f;
        public float hungerDrainRate = 0.6f;
        public float damageFromStarvation = 1.5f;

        [SyncVar(hook = nameof(OnHealthChanged))]
        public float currentHealth;

        [SyncVar(hook = nameof(OnHungerChanged))]
        public float currentHunger;

        public override void OnStartServer()
        {
            base.OnStartServer();
            currentHealth = maxHealth;
            currentHunger = maxHunger;
        }

        [ServerCallback]
        private void Update()
        {
            // only run drain logic on server
            currentHunger = Mathf.Max(0f, currentHunger - hungerDrainRate * Time.deltaTime);

            if (currentHunger <= 0f)
            {
                currentHealth = Mathf.Max(0f, currentHealth - damageFromStarvation * Time.deltaTime);
            }

            if (currentHealth <= 0f)
            {
                // simple respawn on server
                currentHealth = maxHealth;
                currentHunger = maxHunger;

                RpcRespawn();
            }
        }

        [Server]
        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
        }

        [Command]
        public void CmdEatFood(float hungerRestore)
        {
            currentHunger = Mathf.Min(maxHunger, currentHunger + hungerRestore);
            currentHealth = Mathf.Min(maxHealth, currentHealth + 8f);
        }

        private void OnHealthChanged(float oldVal, float newVal)
        {
            // UI update hook point (client side) - implement HUD binding here
        }

        private void OnHungerChanged(float oldVal, float newVal)
        {
            // UI update hook point (client side) - implement HUD binding here
        }

        [ClientRpc]
        private void RpcRespawn()
        {
            // move the player to origin on clients when server respawns
            if (isLocalPlayer)
            {
                transform.position = Vector3.zero;
            }
        }
    }
}
