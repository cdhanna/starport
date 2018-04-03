using Smallgroup.Starport.Assets.Scripts.Events;
using Smallgroup.Starport.Assets.Surface.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSupport : MonoBehaviour {

    public HoverableObject HoverTrigger;
    private float _hideTime;
    private bool _shouldHide;
    public GameObject SelectionCircle;

    private static GameObject _foundSelectionCircle;

    public InteractionGameEvent OnInteractionStartedEvent;
    //[Serializable]
    //public class OnInteractionStartUnityEvent : UnityEvent<Interaction[]> { }
    //public OnInteractionStartUnityEvent OnInteractionStart;

	// Use this for initialization
	void Start () {

        if (SelectionCircle == null)
        {
            if (_foundSelectionCircle == null)
            {

                SelectionCircle = GameObject.FindObjectOfType<SelectionCircle>().gameObject;
                _foundSelectionCircle = SelectionCircle;
            } else
            {
                SelectionCircle = _foundSelectionCircle;
            }
            if (SelectionCircle == null)
            {
                throw new System.Exception("Add a selection circle to the scene!");
            }
           // SelectionCircle.transform.SetParent(transform);
        }

		if (HoverTrigger == null)
        {
            HoverTrigger = GetComponent<HoverableObject>();
        }
        if (HoverTrigger == null)
        {
            HoverTrigger = gameObject.AddComponent<HoverableObject>();
            HoverTrigger.OnHoverStart = new HoverEvent();
            HoverTrigger.OnHoverStop = new UnityEvent();
            HoverTrigger.OnHoverStart.AddListener(color =>
            {
                //_hideTime = Time.realtimeSinceStartup + .25f;
                _shouldHide = false;
                SelectionCircle.transform.position = transform.position;
                //_oldColor = gameObject.GetComponent<MeshRenderer>().material.color;
                //gameObject.GetComponent<MeshRenderer>().material.color = color;
            });
            HoverTrigger.OnHoverStop.AddListener(() =>
           {
                _shouldHide = true;
               _hideTime = Time.realtimeSinceStartup + .25f;
               //SelectionCircle.transform.SetParent(null);
               //gameObject.GetComponent<MeshRenderer>().material.color = _oldColor;
           });
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (_shouldHide && _hideTime > Time.realtimeSinceStartup)
        {

            SelectionCircle.transform.position = new Vector3(0, -100, 0);
        }


    }

    public void StartInteraction()
    {
        Debug.Log("START INTERACTION");
        var all = GetComponents<Interaction>();

        OnInteractionStartedEvent.Raise(all);
        //OnInteractionStart.Invoke(all);

        //Interactions.First().Invoke();
    }

    
}


