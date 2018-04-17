using Smallgroup.Starport.Assets.Scripts.Characters.Commands;
using Smallgroup.Starport.Assets.Scripts.JobSystem;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    
    public class RoomTaskTypeGenerator : MonoBehaviour
    {
        public ActorAnchor ActorAnchor;
        //public GameTaskTypeEventListener Listener;

        private void Start()
        {
            //Listener.on
            if (ActorAnchor == null)
            {
                ActorAnchor = GetComponent<ActorAnchor>();
            }
        }

        public void OnCreate(TaskCreationRequested request)
        {

            var roomType = request.TaskType as RoomTaskType;
            if (roomType == null)
            {
                return;
            }
            var selectionCommand = new BoxSelectionCommand();

            ActorAnchor.Actor.ClearCommands();

            selectionCommand.Callback = () =>
            {
                var isDone = selectionCommand.Selected;
                if (isDone == true)
                {
                    var boxSelection = ActorAnchor.BoxSelector;
                    var rect = new Rect(boxSelection.x, boxSelection.y, boxSelection.width, boxSelection.height);


                    var instance = roomType.CreateInstance();
                    roomType.SetLocation(instance, rect);
                    request.Handler.AddTask(instance);
                }
                
            };

            ActorAnchor.Actor.AddCommand(selectionCommand);

        }


    }
}
