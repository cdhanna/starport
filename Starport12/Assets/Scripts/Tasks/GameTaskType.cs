using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    //[CreateAssetMenu(fileName = "TaskData", menuName = "Jafar/Task")]
    [Serializable]
    public abstract class GameTaskType : ScriptableObject
    {

        public string Title;
        public string Description;
        public string Name { get { return name; } }

        public float GetRatioComplete(GameTask instance)
        {
            return GetRequirements().Select(r => r.GetPercentageComplete(instance)).Sum() / GetRequirements().Count;
        }

        public abstract List<GameTaskParameter> GetParameters();
        public abstract List<RequirementParameter> GetRequirements();

        protected abstract void OnAdvance(GameTask instance);
        protected abstract void OnComplete(GameTask instance);
        protected abstract void OnCreate(GameTask instance);

        public bool IsComplete(GameTask instance)
        {
            // iterate over requirements, and check that they are all done.
            //GetRequirements().ForEach(r => r.)
            return GetRequirements().All(r => r.IsComplete(instance));
        }

        public virtual void Advance(GameTask instance)
        {
            OnAdvance(instance);
            if (IsComplete(instance))
            {
                OnComplete(instance);
            }
        }
        
        public virtual GameTask CreateInstance()
        {
            var task = new GameTask(this);
            OnCreate(task);
            return task;
        }

    }
}
