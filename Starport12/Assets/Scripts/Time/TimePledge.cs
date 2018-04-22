using Smallgroup.Starport.Assets.Scripts.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Time
{
    public interface ITimePledge
    {
        string Name { get; }
        //bool Discrete { get; }

        bool IsDone(Calendar calendar);

    }

    public class DefaultTimePledge : ITimePledge
    {
        public readonly static ITimePledge Idle = new DefaultTimePledge("Idle");

        public string Name { get; private set; }

        public DefaultTimePledge(string name)
        {
            Name = name;
        }

        public virtual bool IsDone(Calendar calendar)
        {
            return false;
        }
    }

    public class TaskTimePledge : DefaultTimePledge
    {
        public GameTask Task { get; private set; }
        public TaskTimePledge(GameTask task) : base("Task " + task.TaskType.Name)
        {
            Task = task;
        }

        public override bool IsDone(Calendar calendar)
        {
            return Task.TaskType.IsComplete(Task);
        }
    }
}
