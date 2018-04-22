using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class ObjectParameter : GameTaskParameter<InteractionSupport>
    {

        public GameObject MustMatchPrefab;

        public override InteractionSupport GetDefault()
        {
            return null;
        }

        public override bool IsValid(GameTask instance, InteractionSupport value, InteractionSupport oldValue)
        {
            if (value == null)
            {
                return false;
            }
            if (MustMatchPrefab == null)
            {
                return true;
            }

            return value.gameObject.name.StartsWith(MustMatchPrefab.name);


            //return base.IsValid(instance, value, oldValue);
        }
    }
}
