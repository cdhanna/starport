using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Surface.Interactions
{
    public class Interaction : MonoBehaviour
    {
        public string DisplayAs;
        public GameObjectGameEvent Event;
        public GameObject arg;

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(DisplayAs) || string.IsNullOrWhiteSpace(DisplayAs))
            {
                return Event.name;
            } else
            {
                return DisplayAs;
            }
        }

        public void Invoke()
        {
            Event.Raise(arg == null ? gameObject : arg);
        }
    }

   
}
