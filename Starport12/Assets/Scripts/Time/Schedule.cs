using Smallgroup.Starport.Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Time
{
    [Serializable]
    public class Schedule : MonoBehaviour
    {

        private List<long> _times = new List<long>();
        private Dictionary<long, ITimePledge> _timeToPledge = new Dictionary<long, ITimePledge>();
        
        //public GameObjectGameEvent OnAdvance;

        public Schedule()
        {

        }

        private void Update()
        {
            
        }

        public bool IsIdleAt(long time)
        {
            return GetActivityAt(time) == DefaultTimePledge.Idle;
        }
        public bool IsIdleNow()
        {
            var calendar = GameObject.FindObjectOfType<WorldAnchor>().Calendar;
            return IsIdleAt(calendar.GetTime());
        }

        public void SetActivityNow(ITimePledge pledge)
        {

            var calendar = GameObject.FindObjectOfType<WorldAnchor>().Calendar;
            SetActivityAt(calendar.GetTime(), pledge);
        }
        public void SetActivityAt(long start, ITimePledge pledge)
        {

            var insertingIntoTime = GetTimeBefore(start);
            if (_times.Contains(start) == false)
            {
                _times.Add(start);
                _timeToPledge.Add(start, pledge);
            } else
            {
                _timeToPledge[start] = pledge;
            }
            _times.Sort();

        }
        public ITimePledge GetActivityNow()
        {
            var calendar = GameObject.FindObjectOfType<WorldAnchor>().Calendar;
            return GetActivityAt(calendar.GetTime());
        }
        public ITimePledge GetActivityAt(long time)
        {
            var knownTime = GetTimeBefore(time);
            if (knownTime == -1)
            {
                return DefaultTimePledge.Idle ;
            }

            return _timeToPledge[knownTime];
        }
        
        

        public long GetTimeBefore(long time)
        {
            for (var i = _times.Count - 1; i > -1; i --)
            {
                var potentialTime = _times[i];
                if (potentialTime <= time)
                {
                    return potentialTime;
                }
            }
            return -1;
        }

        public void CheckForCompletedPledges(Calendar calendar)
        {
            var time = calendar.GetTime();
            var pledge = GetActivityAt(time);
            if (pledge.IsDone(calendar))
            {
                SetActivityAt(calendar.GetTime(), DefaultTimePledge.Idle); // always return to idle
            }
        }

    }

    //public class TimedPledge
    //{
    //    public long Time { get; set; }
    //    public ITimePledge Pledge { get; set; }
    //}
}
