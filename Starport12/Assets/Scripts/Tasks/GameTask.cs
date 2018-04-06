using Smallgroup.Starport.Assets.Scripts.Characters;
using Smallgroup.Starport.Assets.Scripts.GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [Serializable]
    public class GameTask
    {
        public GameTaskType TaskType;
        public List<GameResource> Status;
        public List<JobSlotAssignment> Assignments;

        public GameTask(GameTaskType Type)
        {
            TaskType = Type;
            InitFromTask();
        }

        public GameTask InitFromTask()
        {
            Status = TaskType.RequiredResources.Select(r => new GameResource()
            {
                Amount = 0,
                ResourceType = r.ResourceType
            }).ToList();
            Assignments = TaskType.Slots.Select(s => new JobSlotAssignment()
            {
                Slot = s,
                Character = null
            }).ToList();

            return this;
        }

        public GameTask EnsureStatusMatchesType()
        {
            var currentTypes = Status.Select(s => s.ResourceType).ToList();
            var requiredTypes = TaskType.RequiredResources.Select(r => r.ResourceType).ToList();
            var needTypes = TaskType.RequiredResources
                .Where(req => !currentTypes.Contains(req.ResourceType))
                .ToList();
            var removeTypes = Status
                .Where(s => !requiredTypes.Contains(s.ResourceType))
                .ToList();
            removeTypes.ForEach(t => Status.Remove(t));
            needTypes.ForEach(t => Status.Add(new GameResource()
            {
                ResourceType = t.ResourceType,
                Amount = 0
            }));
            return this;
        }
        
    }
}
