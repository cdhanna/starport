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

    public abstract class GameEvent<TUnityEvent, TEvent, TArg> : ScriptableObject
        where TUnityEvent : UnityEvent<TArg>
        where TEvent : GameEvent<TUnityEvent, TEvent, TArg>
    {
        private List<GameEventListener<TUnityEvent, TEvent, TArg>> _listeners = new List<GameEventListener<TUnityEvent, TEvent, TArg>>();

        public string DisplayName;

        public void Raise(TArg arg)
        {
            Debug.Log("Raising Event " + GetType().Name);
            for (var i = _listeners.Count - 1; i > -1; i--)
            {
                _listeners[i].OnEventRaised(arg);
            }
        }

        public void RegisterListener(GameEventListener<TUnityEvent, TEvent, TArg> listener)
        {
            _listeners.Add(listener);
        }
        public void UnRegisterListener(GameEventListener<TUnityEvent, TEvent, TArg> listener)
        {
            _listeners.Remove(listener);
        }
    }

   
    
}
