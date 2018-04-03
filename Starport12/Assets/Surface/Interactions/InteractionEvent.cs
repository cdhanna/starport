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
    public class InteractionEvent : GameEventListener<InteractionUnityEvent, InteractionGameEvent, Interaction[]> 
    {


    }

    [CreateAssetMenu(fileName = "InteractionsEvent", menuName = "Events/Interactions Type")]
    public class InteractionGameEvent : GameEvent<InteractionUnityEvent, InteractionGameEvent, Interaction[]>
    {
        
    }

    [Serializable]
    public class InteractionUnityEvent : UnityEvent<Interaction[]> { }
}
