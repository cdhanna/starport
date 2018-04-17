using Smallgroup.Starport.Assets.Scripts.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    public bool IsOpen;

    public Text TitleText;
    public Text AboutText;

    private Animator _animator;

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
        TitleText.text = task.TaskType.Title;
        AboutText.text = task.TaskType.Description;
    }

    public void Open()
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }
}
