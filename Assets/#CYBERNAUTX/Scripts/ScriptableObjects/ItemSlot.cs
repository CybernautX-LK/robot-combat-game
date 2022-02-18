using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CybernautX
{
    [CreateAssetMenu(fileName = "ItemSlot", menuName = "CybernautX/Items/ItemSlot", order = 1)]
    public class ItemSlot : ScriptableObject
    {
        public List<Item> availableItems = new List<Item>();
        public Item selectedItem;

        public void UpdateItem(Item item)
        {
            if (!availableItems.Contains(item))
            {
                Debug.LogWarning($"{typeof(ItemSlot).Name}: Can't set item because {item} is not included in this slot's available items.");
                return;
            }

            selectedItem = item;
        }
    }
}
