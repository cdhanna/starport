using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Inventory
{
    class CharacterInventory
    {
        public Dictionary<String, InventoryItem> inventory;

        public CharacterInventory()
        {
            inventory = new Dictionary<string, InventoryItem>();
        }

        public void addToInventory(InventoryItem item)
        {
            bool exists = inventory.ContainsKey(item.name);
            if (exists)
            {
                inventory[item.name].count++;
            }
            else
            {
                inventory.Add(item.name, item);
            }
        }

        public InventoryItem removeFromInventory(String name)
        {
            InventoryItem item;
            bool found = inventory.TryGetValue(name, out item);
            if (found)
            {
                inventory[name].count--;
                if (item.count == 0)
                {
                    inventory.Remove(name);
                }
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}
