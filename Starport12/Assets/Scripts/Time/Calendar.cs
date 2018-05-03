using Smallgroup.Starport.Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Time
{
    public class Calendar : MonoBehaviour
    {

        public const long HOUR = 100;
        public const long HALF_DAY = HOUR * 12;

        [SerializeField]
        private long _time = 0;

        public CalendarEvent AdvanceTimeEvent;
        public CalendarEvent AfterAdvanceTimeEvent;


        public long GetTime()
        {
            return _time;
        }
        public long GetHour()
        {
            return _time / HOUR;
        }
        public long GetHalfDay()
        {
            return _time / HALF_DAY;
        }
        public long GetUpcomingHalfDay(int count = 0)
        {
            return GetHalfDay() + (count + 1) * HALF_DAY;
        }
        public bool IsNight()
        {
            return (GetTime() / HALF_DAY) % 2 == 0;
        }
        public void AdvanceTime(long time)
        {
            _time += time;
            AdvanceTimeEvent.Raise(this);
            AfterAdvanceTimeEvent.Raise(this);
        }

        public void AdvanceTime()
        {
            AdvanceTime(HALF_DAY);
        }

        public static long HoursToLong(int hours)
        {
            return hours * HOUR;
        }

    }
}
