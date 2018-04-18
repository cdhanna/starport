using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.TaskUIHandlers
{
    public class RoomTaskUI : MonoBehaviour, ITaskUIHandler
    {
        public GameObject ResourceLayout;
        public RequirementStatusUI ResourceBarPrefab;

        private GameTask Instance;
        private RoomTaskType Room { get { return Instance.TaskType as RoomTaskType; } }

        public void SetGameTask(GameTask instance)
        {
            Instance = instance;
        }

        public void Start()
        {
            Room.ResourceRequirements.ForEach(req =>
            {
                var bar = Instantiate(ResourceBarPrefab, ResourceLayout.transform);
                bar.ResourceType = req.Resource;
                bar.Total = req.RequiredValue;
            });
        }


    }
}
