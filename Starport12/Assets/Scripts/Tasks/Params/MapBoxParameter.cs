using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class MapBoxParameter : GameTaskParameter
    {
        public MapDataAnchor MatchPattern;
        public bool Loop;

        public override object GetDefault()
        {
            return null;
        }
    }
}
