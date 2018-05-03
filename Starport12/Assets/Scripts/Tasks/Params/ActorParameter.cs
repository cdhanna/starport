using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class ActorParameter : GameTaskParameter<ActorAnchor>
    {
        public long StartOffset = 0;
        public long StopOffset = Calendar.HALF_DAY;

        public bool RequiredToBeHuman;
        public bool RequiredToBeDemon;


        public List<GameResource> RequiredAbilities;

        public override ActorAnchor GetDefault()
        {
            return null;
        }

    

        public override bool IsValid(GameTask instance, ActorAnchor actor, ActorAnchor oldValue)
        {
            if (actor == null) return false;

            var valid = true;

            if (RequiredToBeDemon)
            {
                valid &= actor.Character.IsDemon;

            } else if (RequiredToBeHuman)
            {
                valid &= !actor.Character.IsDemon;
            }

            RequiredAbilities?.ForEach(req =>
            {
                var ability = (actor.Character.ResourceAbilities.FirstOrDefault(r => r.ResourceType == req.ResourceType));
                if (ability == null || ability.Amount < req.Amount)
                {
                    valid = false;
                }
            });

            return valid;
        }

    }
}
