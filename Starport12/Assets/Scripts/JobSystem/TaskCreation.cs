using Smallgroup.Starport.Assets.Scripts.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.JobSystem
{
    public class TaskCreationRequested
    {
        public GameTaskType TaskType;
        public JobHandler Handler;

        public TaskCreationRequested(GameTaskType type, JobHandler handler)
        {
            TaskType = type;
            Handler = handler;
        }


    }
}
