using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Inventory
{
    class InventoryItem
    {
        public String name;
        public int count;
        public  InventoryItem(String name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }
}
