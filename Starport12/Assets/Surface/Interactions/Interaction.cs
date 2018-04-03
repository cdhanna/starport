using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Surface.Interactions
{
    [Serializable]
    public class Interaction
    {
        public string name;
        public UnityEvent OnInvoke;

        public void Invoke()
        {
            OnInvoke.Invoke();
        }
    }

    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners;

        public void Raise()
        {
            for (var i = _listeners.Count-1; i > -1; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }
        public void UnRegisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }

   
}
