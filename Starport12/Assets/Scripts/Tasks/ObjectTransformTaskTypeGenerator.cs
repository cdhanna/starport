using Smallgroup.Starport.Assets.Scripts.Characters.Commands;
using Smallgroup.Starport.Assets.Scripts.JobSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    class ObjectTransformTaskTypeGenerator : MonoBehaviour
    {
        public ActorAnchor ActorAnchor;
        public void OnCreate(TaskCreationRequested request)
        {
            var taskType = request.TaskType as ObjectTransfomTaskType;
            if (taskType == null)
            {
                return;
            }

            var selectCommand = new ObjectSelectionCommand();
            ActorAnchor.Actor.ClearCommands();
            selectCommand.Callback = () =>
            {
                if (selectCommand.Selected)
                {
                    var selected = selectCommand.SelectedObject;

                    var instance = taskType.CreateInstance();
                    if (taskType.ObjectToMoveParameter.IsValid(instance, selected, null))
                    {
                        instance.SetValue(taskType.ObjectToMoveParameter, selectCommand.SelectedObject);
                        request.Handler.AddTask(instance);
                    }

                }
            };
            ActorAnchor.Actor.AddCommand(selectCommand);


        }
    }
}
