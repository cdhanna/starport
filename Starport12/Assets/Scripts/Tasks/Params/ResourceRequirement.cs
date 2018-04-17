﻿using Smallgroup.Starport.Assets.Scripts.GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class ResourceRequirement : RequirementParameter
    {
        public float StartingValue;
        public float RequiredValue;
        public GameResourceType Resource;

        
        public override object GetDefault()
        {
            return StartingValue;
        }

        public override float GetPercentageComplete(GameTask instance)
        {
            var value = (float)instance.GetValue(this);
            return value / RequiredValue;
        }

        //public override void InvokeSetter()
        //{
        //    throw new NotImplementedException();
        //}

        public override bool IsComplete(GameTask instance)
        {
            var value = (float)instance.GetValue(this);
            return value >= RequiredValue;
        }
    }
}
