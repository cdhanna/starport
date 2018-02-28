using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverableObject : MonoBehaviour {

    public HoverEvent OnHoverStart;
    public UnityEvent OnHoverStop;
    public Color HoverColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartHover()
    {
        if (OnHoverStart != null)
        {
            OnHoverStart.Invoke(HoverColor);
        }
    }

    public void StopHover()
    {
        if (OnHoverStop != null)
        {
            OnHoverStop.Invoke();
        }
    }
}
