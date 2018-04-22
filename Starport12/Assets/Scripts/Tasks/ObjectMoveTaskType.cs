using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [CreateAssetMenu(fileName = "MoveTaskData", menuName = "Jafar/Tasks/MoveObject")]
    public class ObjectMoveTaskType : GameTaskType
    {

        public ObjectParameter ObjectToMoveParameter;
        public MapBoxParameter LocationToGoParameter;
        public List<ActorParameter> Assignments;

        public List<TimeRequirement> TimeRequirements;

        public override List<IGameTaskParameter> GetParameters()
        {
            var set = new List<IGameTaskParameter>();
            set.Add(ObjectToMoveParameter);
            set.Add(LocationToGoParameter);
            set.AddRange(Assignments);
            return set;
        }

        public override List<RequirementParameter> GetRequirements()
        {
            var set = new List<RequirementParameter>();
            set.AddRange(TimeRequirements);
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
        }
        

        protected override void OnComplete(GameTask instance)
        {
            var coord = instance.GetValue(LocationToGoParameter).GetCoordinates()[0];
            var pos = FindObjectOfType<WorldAnchor>().Map.TransformCoordinateToWorld(coord);
            instance.GetValue(ObjectToMoveParameter).transform.position = pos;
        }

        protected override void OnCreate(GameTask instance)
        {
            
        }
    }
}
