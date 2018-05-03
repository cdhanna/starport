using Smallgroup.Starport.Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Smallgroup.Starport.Assets.Scripts.Time
{

    [Serializable]
    public class CalendarEventListener : GameEventListener<CalendarUnityEvent, CalendarEvent, Calendar>
    {
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Calendar Event", menuName = "Events/Calendar Type")]
    public class CalendarEvent : GameEvent<CalendarUnityEvent, CalendarEvent, Calendar>
    {

    }

    [Serializable]
    public class CalendarUnityEvent : UnityEvent<Calendar>
    {

    }
}
