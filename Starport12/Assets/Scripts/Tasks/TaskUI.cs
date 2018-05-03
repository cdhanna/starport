using Smallgroup.Starport.Assets.Scripts.JobSystem;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    public bool IsOpen;

    public Text TitleText;
    public Text AboutText;
    public GameObject ChildPanel;

    //public GameTaskEvent CloseIfTaskDestroyed;

    private Animator _animator;
    private GameTask _task;

    private GameObject childInstance;

	// Use this for initialization
	void Start () {

        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        _animator.SetBool("isOpen", IsOpen);
	}

    public void SetForTask(GameTask task)
    {
        _task = task;
        TitleText.text = task.TaskType.Title;
        AboutText.text = task.TaskType.Description;

        var handlers = GetComponents<TaskUIHandler>().ToList();
        var handler = handlers.First(h => h.TaskTypeExample.GetType().IsAssignableFrom(task.TaskType.GetType()));
        if (childInstance != null)
        {
            Destroy(childInstance);
            childInstance = null;
        }
        childInstance = handler.CreateUI(task, ChildPanel);

    }

    public void Open()
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }

    public void DestroyIfSelf(GameTask task)
    {
        if (_task == task)
        {
            Close();

        }
    }
}
