using Smallgroup.Starport.Assets.Scripts.Events;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.JobSystem
{
    public class JobHandler : MonoBehaviour
    {

        public GameTaskRequestEvent OnTaskTypeAttempted;

        public List<GameTask> Tasks;

        public GameTaskEvent OnAddedGameTask;
        public GameTaskEvent OnCompletedGameTask;

        public void AdvanceJobs()
        {
            var completed = new List<GameTask>();
            foreach(var task in Tasks)
            {
                task.TaskType.Advance(task);
                if (task.TaskType.IsComplete(task))
                {
                    completed.Add(task);
                }
            }

            completed.ForEach(task =>
            {
                OnCompletedGameTask?.Raise(task);
            });
        }

        public void TryGenerate(GameTaskType taskType)
        {
            OnTaskTypeAttempted.Raise(new TaskCreationRequested(taskType, this));
          
        }

        public void AddTask(GameTask instance)
        {
            Tasks.Add(instance);
            OnAddedGameTask.Raise(instance);
        }

        public void RemoveTask(GameTask instance)
        {
            Tasks.Remove(instance);
        }

        public List<GameTask> GetTasks()
        {
            return Tasks.ToList();
        }

    }
}
