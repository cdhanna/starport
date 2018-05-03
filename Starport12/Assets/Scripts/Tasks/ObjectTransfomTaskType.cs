using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [CreateAssetMenu(fileName = "TransformTaskData", menuName = "Jafar/Tasks/TransformObject")]
    public class ObjectTransfomTaskType : GameTaskType
    {
        public GameObject TransformInto;

        public ObjectParameter ObjectToMoveParameter;
        public List<ActorParameter> Assignments;
        public List<TimeRequirement> TimeRequirements;
        public List<ResourceRequirement> ResourceRequirements;


        public override List<IGameTaskParameter> GetParameters()
        {
            var set = new List<IGameTaskParameter>();
            set.Add(ObjectToMoveParameter);
            set.AddRange(Assignments);
            return set;
        }

        public override List<RequirementParameter> GetRequirements()
        {
            var set = new List<RequirementParameter>();
            set.AddRange(TimeRequirements);
            set.AddRange(ResourceRequirements);

            return set;
        }

        protected override void OnAdvance(GameTask instance)
        {

            var anyAssignee = Assignments.Select(req => instance.GetValue(req).Actor).Any(a => a != null);

            if (anyAssignee)
            {

                TimeRequirements.ForEach(req =>
                {
                    var value = instance.GetValue(req);
                    instance.SetValue(req, value - 1);
                });
            }

            // get the actor assignments, and bake out the resources that they yield.
            var resourceValues = new Dictionary<GameResourceType, float>();
            Assignments.ForEach(assignment =>
            {
                var actor = instance.GetValue(assignment);
                actor?.Character?.ResourceAbilities.ForEach(ability =>
                {
                    if (resourceValues.ContainsKey(ability.ResourceType) == false)
                    {
                        resourceValues.Add(ability.ResourceType, ability.Amount);
                    }
                    else
                    {
                        resourceValues[ability.ResourceType] += ability.Amount;
                    }
                });
            });


            ResourceRequirements.ForEach(req =>
            {
                var currentValue = instance.GetValue(req);
                float add = 0;
                if (resourceValues.TryGetValue(req.Resource, out add))
                {


                    instance.SetValue(req, Math.Min(req.RequiredValue, currentValue + add));
                }
            });
        }


        protected override void OnComplete(GameTask instance)
        {
            var selected = instance.GetValue(ObjectToMoveParameter);
            var obj = Instantiate(TransformInto, selected.transform.parent);
            obj.transform.localPosition = selected.transform.localPosition;
            obj.transform.localRotation = selected.transform.localRotation;
            FindObjectOfType<WorldAnchor>().RebuildNav();
            Destroy(selected.gameObject);
        }

        protected override void OnCreate(GameTask instance)
        {
        }
    }
}
