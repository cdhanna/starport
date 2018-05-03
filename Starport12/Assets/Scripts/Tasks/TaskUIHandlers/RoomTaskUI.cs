using Smallgroup.Starport.Assets.Scripts.Tasks.Params;
using Smallgroup.Starport.Assets.Scripts.Time;
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

        public GameObject AssignLayout;
        public CharacterSelectUI CharacterSelectPrefab;


        private GameTask Instance;
       // private ActorAnchor Actor;
        private RoomTaskType Room { get { return Instance.TaskType as RoomTaskType; } }


        private Dictionary<ResourceRequirement, RequirementStatusUI> _req2Ui = new Dictionary<ResourceRequirement, RequirementStatusUI>();

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
                bar.Actual = (float) Instance.GetValue<float>(req);
                _req2Ui.Add(req, bar);
            });

            var calendar = GameObject.FindObjectOfType<WorldAnchor>().Calendar;

            Room.Assignments.ForEach(assignment =>
            {
                var ui = Instantiate(CharacterSelectPrefab, AssignLayout.transform);
                ui.ActorAllowed = (a) => assignment.IsValid(Instance, a, null);
                var existing = Instance.GetValue(assignment);
                ui.SelectActor(existing);
                ui.OnActorSelected += (s, nextActor) =>
                {
                    var oldActor = Instance.GetValue(assignment);

                    if (oldActor != null)
                    {
                        oldActor.Schedule.SetActivityNow(DefaultTimePledge.Idle);
                    }
                    if (nextActor == null)
                    {

                        Instance.SetValue(assignment, null);

                    }
                    else
                    {


                        var isIdle = nextActor.Schedule.IsIdleNow();
                        if (isIdle)
                        {
                            nextActor.Schedule.SetActivityNow(new TaskTimePledge(Instance));
                            Instance.SetValue(assignment, nextActor);
                        }
                        else
                        {
                            ui.SelectActor(null);
                        }
                    }

                };

            });
        }

        public void Update()
        {
            Room.ResourceRequirements.ForEach(req =>
            {
                var bar = _req2Ui[req];
                bar.Total = req.RequiredValue;
                bar.Actual = (float)Instance.GetValue<float>(req);
               
            });
        }


    }
}
