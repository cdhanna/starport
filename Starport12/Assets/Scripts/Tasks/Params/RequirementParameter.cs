using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public abstract class RequirementParameter : GameTaskParameter<float>
    {
        public abstract bool IsComplete(GameTask instance);
        public abstract float GetPercentageComplete(GameTask instance);

      
        //public abstract object GetDefault();
        //object GetDefault();
        //float GetRatioComplete(TInput input);
    }
}
