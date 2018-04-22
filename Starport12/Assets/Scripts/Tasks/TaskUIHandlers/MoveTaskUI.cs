using Smallgroup.Starport.Assets.Scripts.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.TaskUIHandlers
{
    public class MoveTaskUI : MonoBehaviour, ITaskUIHandler
    {
        private GameTask Instance;

        public GameObject AssignLayout;
        public CharacterSelectUI CharacterSelectPrefab;
        private ObjectMoveTaskType ObjectMove { get { return Instance.TaskType as ObjectMoveTaskType; } }

        public void SetGameTask(GameTask instance)
        {
            Instance = instance;
        }

        void Start()
        {
            ObjectMove.Assignments.ForEach(assignment =>
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
    }
}
