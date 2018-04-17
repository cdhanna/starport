using Smallgroup.Starport.Assets.Scripts.Characters;
using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [Serializable]
    public class GameTask
    {
        public GameTaskType TaskType;

        private Dictionary<object, object> _currentValues;
        

        public GameTask(GameTaskType Type)
        {
            TaskType = Type;
            _currentValues = new Dictionary<object, object>();

            Type.GetRequirements().ForEach(r => _currentValues.Add(r, r.GetDefault()));
            Type.GetParameters().ForEach(p => _currentValues.Add(p, p.GetDefault()));
        }

        public virtual bool HasValue(GameTaskParameter parameter)
        {
            return _currentValues.ContainsKey(parameter);
        }
        public virtual void SetValue(GameTaskParameter parameter, object value)
        {
            if (HasValue(parameter))
            {
                _currentValues[parameter] = value;
            } else
            {
                _currentValues.Add(parameter, value);
            }
        }
        public virtual object GetValue(GameTaskParameter parameter)
        {
            if (_currentValues.ContainsKey(parameter))
            {
                return _currentValues[parameter];
            } else
            {
                throw new Exception("No parameter value exists for " + parameter.Name);
            }
        }        


        public bool IsValid()
        {
            return !TaskType.GetParameters()
                    .Where(p => p.RequiredToCreateInstance)
                    .Any(p => !HasValue(p));
        }

        /*
         * 
         * var params = task.TaskType.GetAllParameters()
         * 
         * var values = params.Select(p => task.GetValue(p) );
         * 
         */

        //public GameTaskParameterValue<T> GetParameterValue<T>(GameTaskParameter<T> parameter)
        //{

        //}

        //public GameTask InitFromTask()
        //{
        //    Status = TaskType.RequiredResources.Select(r => new GameResource()
        //    {
        //        Amount = 0,
        //        ResourceType = r.ResourceType
        //    }).ToList();
        //    Assignments = TaskType.Slots.Select(s => new JobSlotAssignment()
        //    {
        //        Slot = s,
        //        Character = null
        //    }).ToList();

        //    return this;
        //}

        //public GameTask EnsureStatusMatchesType()
        //{
        //    var currentTypes = Status.Select(s => s.ResourceType).ToList();
        //    var requiredTypes = TaskType.RequiredResources.Select(r => r.ResourceType).ToList();
        //    var needTypes = TaskType.RequiredResources
        //        .Where(req => !currentTypes.Contains(req.ResourceType))
        //        .ToList();
        //    var removeTypes = Status
        //        .Where(s => !requiredTypes.Contains(s.ResourceType))
        //        .ToList();
        //    removeTypes.ForEach(t => Status.Remove(t));
        //    needTypes.ForEach(t => Status.Add(new GameResource()
        //    {
        //        ResourceType = t.ResourceType,
        //        Amount = 0
        //    }));
        //    return this;
        //}
        
    }
}
