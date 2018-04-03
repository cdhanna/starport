using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Surface.Interactions
{
    public class GameEventListener : MonoBehaviour
    {
        public UnityEvent OnRaised;
        public GameEvent Event;

        public void Awake()
        {
            Event.RegisterListener(this);
        }
        public void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        public virtual void OnEventRaised()
        {
            OnRaised.Invoke();
        }
    }
}
