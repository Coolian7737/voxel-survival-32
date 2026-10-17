using UnityEngine;

namespace VoxelPrototype
{
    public class ProjectileDamage : MonoBehaviour
    {
        private float damage = 10f;
        private float lifeTime = 3f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        public void SetDamage(float amount)
        {
            damage = amount;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var survival = collision.gameObject.GetComponent<SurvivalSystem>();
            if (survival != null)
            {
                survival.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
