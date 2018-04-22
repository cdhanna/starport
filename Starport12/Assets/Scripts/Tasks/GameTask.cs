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

        //private Dictionary<object, float> _currentValues;


        private Dictionary<Type, Dictionary<object, object>> _typedValueTables;


        public GameTask(GameTaskType Type)
        {
            TaskType = Type;
            //_currentValues = new Dictionary<object, float>();
            _typedValueTables = new Dictionary<System.Type, Dictionary<object, object>>();

            Type.GetRequirements().ForEach(r => r.OnCreateInstance(this));
            Type.GetParameters().ForEach(r => r.OnCreateInstance(this));
            //Type.GetRequirements().ForEach(r => SetValue<>)
            //Type.GetRequirements().ForEach(r => _currentValues.Add(r, r.GetDefault()));
            //Type.GetParameters().ForEach(p => _currentValues.Add(p, p.GetDefault()));
        }

        //public virtual bool HasValue(GameTaskParameter parameter)
        //{
        //    return _currentValues.ContainsKey(parameter);
        //}
        public virtual bool HasValue<T>(GameTaskParameter<T> parameter)
        {
            if (_typedValueTables.ContainsKey(typeof(T)) == false)
            {
                _typedValueTables.Add(typeof(T), new Dictionary<object, object>());
            }

            var table = _typedValueTables[typeof(T)];
            return table.ContainsKey(parameter);
            
        }

        public virtual void SetValue<T>(GameTaskParameter<T> parameter, T value)
        {
            if (_typedValueTables.ContainsKey(typeof(T)) == false)
            {
                _typedValueTables.Add(typeof(T), new Dictionary<object, object>());
            }

            var table = _typedValueTables[typeof(T)];
            if (table.ContainsKey(parameter))
            {
                var oldValue = table[parameter];
                if (parameter.IsValid(this, value, (T)oldValue))
                {
                    table[parameter] = value;
                }
            } else
            {
                if (parameter.IsValid(this, value, default(T)))
                {
                    table.Add(parameter, value);
                }
            }
        }
        public virtual T GetValue<T>(GameTaskParameter<T> parameter)
        {
            if (_typedValueTables.ContainsKey(typeof(T)) == false)
            {
                _typedValueTables.Add(typeof(T), new Dictionary<object, object>());
            }
            var table = _typedValueTables[typeof(T)];
            if (table.ContainsKey(parameter))
            {
                return (T)table[parameter];
            }
            else
            {
                return parameter.GetDefault();
                //throw new Exception("No parameter value exists for " + parameter.Name);

            }
        }

        //public virtual void SetValue(GameTaskParameter parameter, float value)
        //{
        //    if (HasValue(parameter))
        //    {
        //        _currentValues[parameter] = value;
        //    } else
        //    {
        //        _currentValues.Add(parameter, value);
        //    }
        //}
        //public virtual float GetValue(GameTaskParameter parameter)
        //{
        //    if (_currentValues.ContainsKey(parameter))
        //    {
        //        return _currentValues[parameter];
        //    } else
        //    {
        //        throw new Exception("No parameter value exists for " + parameter.Name);
        //    }
        //}        


        //public bool IsValid()
        //{
        //    return !TaskType.GetParameters()
        //            .Where(p => p.RequiredToCreateInstance)
        //            .Any(p => !HasValue(p));
        //}

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
