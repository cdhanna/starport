using Smallgroup.Starport.Assets.Scripts.GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class ResourceCommitParameter : GameTaskParameter<float>
    {

        public GameResourceType TargetResource;

        public override float GetDefault()
        {
            return 1;
        }
    }
}
