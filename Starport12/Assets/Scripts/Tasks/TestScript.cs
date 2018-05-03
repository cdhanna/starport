//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Smallgroup.Starport.Assets.Scripts.Tasks
//{
//    class TestScript : MonoBehaviour
//    {

//        public List<GameTaskType> TasksTypes;

//        public List<GameTask> Tasks;

//        public int selected = 0;
//        public TaskUI UI;

//        private int _prevSelected = -1;

//        public void Start()
//        {
//            Tasks = TasksTypes.Select(s => s.CreateInstance()).ToList();
//        }

//        public void Update()
//        {

//            selected = selected % Tasks.Count;
//            if (_prevSelected != selected)
//            {

//                UI.Task = Tasks[selected];

//                _prevSelected = selected ;
//            }
//        }

//    }
//}
