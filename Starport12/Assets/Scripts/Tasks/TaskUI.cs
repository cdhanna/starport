using Smallgroup.Starport.Assets.Scripts.GameResources;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    public GameTask Task;

    public Text TitleText;
    public Text FlavorText;

    public RequirementStatusUI StatusTemplate;
    public SlotUI SlotTemplate;

    private List<GameResource> _resources;
    private Dictionary<GameResource, RequirementStatusUI> _resource2UI = new Dictionary<GameResource, RequirementStatusUI>();

    //private Dictionary<JobSlot, >
    private Dictionary<JobSlot, SlotUI> _slot2UI = new Dictionary<JobSlot, SlotUI>();


    private GameTask _oldTask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
       
        if (Task != null)
        {
            if (_oldTask != Task)
            {
                Debug.Log("CHANGING TASK DISPLAY FROM " + ((_oldTask == null) ? "null" : _oldTask.TaskType.name) + " TO " + ((Task == null) ? "null" : Task.TaskType.name));
                var type = Task.TaskType;
                
                TitleText.text = type.Name;
                FlavorText.text = type.Description;

                UpdateReqs();


                UpdateSlots();


                _oldTask = Task;
            }
        }

	}

    void UpdateSlots()
    {
        _slot2UI.Values.ToList().ForEach(v => Destroy(v.gameObject));
        _slot2UI.Clear();
        if (Task.Assignments == null || Task.Assignments.Count == 0)
        {
            return;
        }
        for (var i = 0; i < Task.TaskType.Slots.Count; i++)
        {
            var slot = Task.TaskType.Slots[i];
            var assignment = Task.Assignments.First(a => a.Slot == slot);

            var clone = Instantiate<GameObject>(SlotTemplate.gameObject, SlotTemplate.transform.parent);
            clone.SetActive(true);
            var clonedUI = clone.GetComponent<SlotUI>();
            clonedUI.SlotAssignment = assignment;
            
            var rect = clone.transform as RectTransform;
            clone.transform.localPosition += new Vector3(i * (rect.rect.width + 7), 0, 0);
            _slot2UI.Add(slot, clonedUI);
        }
    }

    void UpdateReqs()
    {
        if (_resources != Task.TaskType.RequiredResources)
        {
            // get rid of any old 
            _resource2UI.Values.ToList().ForEach(v => Destroy(v.gameObject));
            _resource2UI.Clear();

            Task.EnsureStatusMatchesType();

            _resources = Task.TaskType.RequiredResources;

            for (var i = 0; i < _resources.Count; i++)
            {
                var resource = _resources[i];
                var currentResource = Task.Status.First(r => r.ResourceType.Equals(resource.ResourceType));

                var clone = Instantiate<GameObject>(StatusTemplate.gameObject, StatusTemplate.transform.parent);
                clone.SetActive(true);
                var clonedUI = clone.GetComponent<RequirementStatusUI>();
                clonedUI.BaseResource = resource;

                clonedUI.ActualResource = currentResource;
                var rect = clone.transform as RectTransform;
                clone.transform.localPosition += new Vector3(i * (rect.rect.width + 7), 0, 0);
                _resource2UI.Add(resource, clonedUI);
            }
        }

    }

}
