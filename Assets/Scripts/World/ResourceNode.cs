using UnityEngine;

namespace VoxelPrototype
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Fiber
    }

    public class ResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;
        [SerializeField] private int hitsToBreak = 3;
        [SerializeField] private int amountPerHit = 1;

        private int remainingHits;

        private void Start()
        {
            remainingHits = hitsToBreak;
        }

        public void Harvest()
        {
            if (remainingHits <= 0)
            {
                return;
            }

            remainingHits -= 1;

            var crafting = FindObjectOfType<CraftingSystem>();
            if (crafting != null)
            {
                crafting.AddItem(ResourceTypeToString(resourceType), amountPerHit);
            }

            if (remainingHits <= 0)
            {
                Destroy(gameObject);
            }
        }

        private static string ResourceTypeToString(ResourceType type)
        {
            return type switch
            {
                ResourceType.Wood => "wood",
                ResourceType.Stone => "stone",
                ResourceType.Fiber => "fiber",
                _ => "resource"
            };
        }
    }
}
