using Smallgroup.Starport.Assets.Scripts.JobSystem;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListUI : MonoBehaviour {

    public TaskRowUI TaskRowGameObjectPrefab;
    public GameObject RowParent;
    public bool IsOpen;
    private Dictionary<GameTask, TaskRowUI> _taskToUI = new Dictionary<GameTask, TaskRowUI>();


    private Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        _animator.SetBool("isOpen", IsOpen);
	}

    //public void ShowScreen()
    //{
    //    GetComponent<Animator>().SetBool("isOpen", true);
    //}
    //public void HideScreen()
    //{
    //    GetComponent<Animator>().SetBool("isOpen", true);
    //}

    public void AddGameTask(GameTask task)
    {
        var rowUI = GameObject.Instantiate(TaskRowGameObjectPrefab, RowParent.transform);
        rowUI.Task = task;
        _taskToUI.Add(task, rowUI);
        Debug.Log("Created UI for " + task.TaskType.Description);
    }

    public void RemoveGameTask(GameTask task)
    {
        var ui = default(TaskRowUI);
        if (_taskToUI.TryGetValue(task, out ui))
        {
            Debug.Log("Removed UI for " + task.TaskType.Description);
            Destroy(ui.gameObject);
            _taskToUI.Remove(task);
        }
    }

    public void Close()
    {
        IsOpen = false;
    }
    public void Open()
    {
        IsOpen = true;
    }

}
