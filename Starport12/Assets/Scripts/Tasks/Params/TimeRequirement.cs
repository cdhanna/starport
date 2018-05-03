using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class TimeRequirement : RequirementParameter
    {

        public float RequiredDuration;



        public override float GetDefault()
        {
            return RequiredDuration;
        }

        public override float GetPercentageComplete(GameTask instance)
        {
            var value = instance.GetValue<float>(this);
            return 1-(value / RequiredDuration);
        }

        public override bool IsComplete(GameTask instance)
        {
            var value = instance.GetValue<float>(this);
            return value <= 0;
        }
    }
}
