using Smallgroup.Starport.Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Scripts.Events
{
    public class GameEventListener<TUnityEvent, TEvent, TArg> : MonoBehaviour
        where TUnityEvent : UnityEvent<TArg>
        where TEvent : GameEvent<TUnityEvent, TEvent, TArg>
    {
        public TUnityEvent OnRaised;
        public TEvent Event;

        public void Awake()
        {
            Event.RegisterListener(this);
        }
        public void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        public void OnEventRaised(TArg arg)
        {
            //       OnRaised.
            OnRaised.Invoke(arg);
        }
    }

    
}
