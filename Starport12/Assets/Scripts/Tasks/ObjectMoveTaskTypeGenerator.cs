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
    public class ObjectMoveTaskTypeGenerator : MonoBehaviour
    {
        public ActorAnchor ActorAnchor;
        public void OnCreate(TaskCreationRequested request)
        {
            var taskType = request.TaskType as ObjectMoveTaskType;
            if (taskType == null)
            {
                return;
            }

            var selectCommand = new ObjectSelectionCommand();
            ActorAnchor.Actor.ClearCommands();

            selectCommand.Callback = () =>
            {
                if (selectCommand.SelectedObject != null)
                {


                    var boxCommand = taskType.LocationToGoParameter.GenerateCommand();
                    //ActorAnchor.Actor.ClearCommands();
                    ActorAnchor.Actor.AddCommand(boxCommand);



                    boxCommand.Callback = () =>
                    {
                        if (boxCommand.Selected)
                        {

                            var instance = taskType.CreateInstance();
                            instance.SetValue(taskType.ObjectToMoveParameter, selectCommand.SelectedObject);
                            instance.SetValue(taskType.LocationToGoParameter, ActorAnchor.BoxSelector.GetResult());
                            request.Handler.AddTask(instance);


                        }
                    };
                }
            };

            ActorAnchor.Actor.AddCommand(selectCommand);

        }

    }
}
