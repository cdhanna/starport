using Smallgroup.Starport.Assets.Scripts.Events;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Scripts.JobSystem
{
    [Serializable]
    public class GameTaskRequestUnityEvent : UnityEvent<TaskCreationRequested>
    {
    }


    [CreateAssetMenu(fileName = "GameTaskTypeRequest Event", menuName = "Events/GameTaskRequest Type")]
    public class GameTaskRequestEvent : GameEvent<GameTaskRequestUnityEvent, GameTaskRequestEvent, TaskCreationRequested>
    {

    }

}
