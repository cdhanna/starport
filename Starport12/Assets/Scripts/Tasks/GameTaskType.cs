using Smallgroup.Starport.Assets.Scripts.GameResources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    [CreateAssetMenu(fileName = "TaskData", menuName = "Jafar/Task")]
    public class GameTaskType : ScriptableObject
    {


        public string Description;
        public string Name { get { return name; } }
        public List<JobSlot> Slots;
        public List<GameResource> RequiredResources;


        public GameTask CreateInstance()
        {
            var task = new GameTask(this);
           
            return task;
        }

    }
}
