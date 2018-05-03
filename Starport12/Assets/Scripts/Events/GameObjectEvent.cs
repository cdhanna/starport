using Smallgroup.Starport.Assets.Surface.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Scripts.Events
{

    public class GameObjectEvent : GameEventListener<GameObjectUnityEvent, GameObjectGameEvent, GameObject>
    {

    }

    [CreateAssetMenu(fileName = "GameObjectEvent", menuName = "Events/GameObject Type")]
    public class GameObjectGameEvent : GameEvent<GameObjectUnityEvent, GameObjectGameEvent, GameObject>
    {

    }

    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject>
    {

    }
}
