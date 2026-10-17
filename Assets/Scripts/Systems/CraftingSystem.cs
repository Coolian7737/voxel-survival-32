using System.Collections.Generic;
using UnityEngine;

namespace VoxelPrototype
{
    public class CraftingSystem : MonoBehaviour
    {
        private readonly Dictionary<string, int> inventory = new Dictionary<string, int>();

        private readonly Dictionary<string, Dictionary<string, int>> recipes = new Dictionary<string, Dictionary<string, int>>
        {
            { "stone_pickaxe", new Dictionary<string, int> { { "wood", 2 }, { "stone", 3 } } },
            { "campfire", new Dictionary<string, int> { { "wood", 4 }, { "stone", 2 } } },
            { "bandage", new Dictionary<string, int> { { "fiber", 2 } } }
        };

        public bool TryCraft(string recipeName)
        {
            if (!recipes.TryGetValue(recipeName, out var recipe))
            {
                return false;
            }

            foreach (var pair in recipe)
            {
                if (!inventory.ContainsKey(pair.Key) || inventory[pair.Key] < pair.Value)
                {
                    return false;
                }
            }

            foreach (var pair in recipe)
            {
                inventory[pair.Key] -= pair.Value;
            }

            inventory[recipeName] = inventory.ContainsKey(recipeName) ? inventory[recipeName] + 1 : 1;
            return true;
        }

        public void AddItem(string itemName, int amount)
        {
            if (inventory.ContainsKey(itemName))
            {
                inventory[itemName] += amount;
            }
            else
            {
                inventory.Add(itemName, amount);
            }
        }

        public int GetItemCount(string itemName)
        {
            return inventory.TryGetValue(itemName, out var count) ? count : 0;
        }
    }
}
