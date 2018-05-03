using Smallgroup.Starport.Assets.Scripts.Events;
using Smallgroup.Starport.Assets.Scripts.Tasks;
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
    public class GameTaskEventListener : GameEventListener<GameTaskUnityEvent, GameTaskEvent, GameTask>
    {
    }

    [Serializable]
    [CreateAssetMenu(fileName = "GameTask Event", menuName = "Events/GameTask Type")]
    public class GameTaskEvent : GameEvent<GameTaskUnityEvent, GameTaskEvent, GameTask>
    {
        public bool Silent = false;
    }

    [Serializable]
    public class GameTaskUnityEvent : UnityEvent<GameTask>
    {

    }
}
