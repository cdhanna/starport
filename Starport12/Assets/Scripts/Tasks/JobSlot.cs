using Smallgroup.Starport.Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [Serializable]
    public class JobSlot
    {

        // todo add constraints?
      
    }

    [Serializable]
    public class JobSlotAssignment
    {
        public JobSlot Slot;
        public CharacterData Character;
    }
}
