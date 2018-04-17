using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [CreateAssetMenu(fileName = "RoomTaskData", menuName = "Jafar/Tasks/Room")]
    public class RoomTaskType : GameTaskType
    {
        public string FlavorText;
        public MapBoxParameter LocationRequirement;
        public List<ResourceRequirement> ResourceRequirements;
        public List<ResourceCommitParameter> ResourceCommitments;
        public MapDataAnchor OutputMFT;

        protected override void OnCreate(GameTask instance)
        {
            // issue the box select command...
            //LocationRequirement.
        }

        protected override void OnComplete(GameTask instance)
        {
            // yahoo?
        }

        protected override void OnAdvance(GameTask instance)
        {
            // advance resourceCommitments

            // match the commits to the reqs.
            ResourceRequirements.ForEach(req =>
            {
                // is there a commit?
                var commit = ResourceCommitments.FirstOrDefault(c => c.TargetResource == req.Resource);
                if (commit != null)
                {
                    var currentValue = (float) instance.GetValue(req);
                    var addValue = (float)instance.GetValue(commit);
                    var nextValue = currentValue + addValue;
                    instance.SetValue(req, addValue + nextValue);
                }
            });

        }

        public override List<GameTaskParameter> GetParameters()
        {
            var parameters = new List<GameTaskParameter>();
            parameters.Add(LocationRequirement);
            parameters.AddRange(ResourceCommitments);
            return parameters;
        }

        public override List<RequirementParameter> GetRequirements()
        {
            var parameters = new List<RequirementParameter>();
            parameters.AddRange(ResourceRequirements);
            return parameters;
        }

        public void SetLocation(GameTask instance, Rect location)
        {
            instance.SetValue(LocationRequirement, location);
        }
       
    }
}
