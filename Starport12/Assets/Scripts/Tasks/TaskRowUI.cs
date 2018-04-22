using Smallgroup.Starport.Assets.Scripts.JobSystem;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskRowUI : MonoBehaviour {

    public GameTask Task;
    public GameTaskEvent RequestCancelTaskEvent;
    public GameTaskEvent RequestInspectTaskEvent;

    public Text RatioComplete;
    public Text Description;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Task != null)
        {
            RatioComplete.text = Mathf.Floor(100*Task.TaskType.GetRatioComplete(Task)) + "%";
            Description.text = Task.TaskType.Description;
        }

	}

    public void OnClose()
    {
        RequestCancelTaskEvent.Raise(Task);
    }

    public void Inspect()
    {
        RequestInspectTaskEvent.Raise(Task);
    }
}
