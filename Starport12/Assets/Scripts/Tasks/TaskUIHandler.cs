using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    public class TaskUIHandler : MonoBehaviour
    {
        public GameObject PanelPrefab;
        public GameTaskType TaskTypeExample;

        internal GameObject CreateUI(GameTask instance, GameObject parent)
        {
            if (PanelPrefab == null)
            {
                Debug.LogError("Task UI Handler didnt have a panel");
                return null;
            }
            var gob = Instantiate(PanelPrefab, parent.transform);
            gob.GetComponent<ITaskUIHandler>().SetGameTask(instance);
            return gob;
        }
    }

    public interface ITaskUIHandler
    {
        void SetGameTask(GameTask instance);
    }
}
