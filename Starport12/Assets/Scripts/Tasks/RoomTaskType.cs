using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.MapSelect;
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
        //public List<ResourceCommitParameter> ResourceCommitments;
        public List<ActorParameter> Assignments;
        public MapDataAnchor OutputMFT;

        protected override void OnCreate(GameTask instance)
        {
            // issue the box select command...
            //LocationRequirement.
        }

        protected override void OnComplete(GameTask instance)
        {
            // yahoo?

            // modify the map, now motherfucker.
            var selection = instance.GetValue(LocationRequirement);
            var World = selection.World;
            var setToWalk = selection.GetCoordinates(0);
            setToWalk.ForEach(c =>
            {
                selection.World.Map.Handlers.Walkable.Set(World.Map.GetCell(c), true);

            });

            var toKill = selection.OverlappingObjects;
            toKill.ForEach(g =>
            {
                DestroyImmediate(g);
            });
            World.Map.AutoMap((coord, cell) => World.Map.Handlers.Walkable.Process(cell));

            var results = MapLoader.ApplyRules(World.Results.Global, World.Map, new PatternSet(), new List<Surface.Generation.Rules.SuperRule>(), setToWalk);
            World.Results.Join(results);
            World.RebuildNav();

        }

        protected override void OnAdvance(GameTask instance)
        {
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
                    } else
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

        

        public override List<IGameTaskParameter> GetParameters()
        {

            var parameters = new List<IGameTaskParameter>();
            parameters.Add(LocationRequirement);
            parameters.AddRange(Assignments);
            //var reqs = ResourceCommitments.Select(r => r.AsDumb()).ToList(); ;
            //parameters.AddRange(reqs);
            //parameters.AddRange(ResourceCommitments);
            return parameters;
        }

        public override List<RequirementParameter> GetRequirements()
        {
            var parameters = new List<RequirementParameter>();
            parameters.AddRange(ResourceRequirements);
            return parameters;
        }

        public void SetLocation(GameTask instance, MapSelection location)
        {
            instance.SetValue(LocationRequirement,  location.GetResult());
        }
       
    }
}
