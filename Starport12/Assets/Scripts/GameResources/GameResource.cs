using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.GameResources
{
    [Serializable]
    public class GameResource
    {
        //public string name;
        public GameResourceType ResourceType;
        public int Amount;
        //public bool Assignable = true;
    }
}
