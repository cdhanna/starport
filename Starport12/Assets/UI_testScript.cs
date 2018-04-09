using Smallgroup.Starport.Assets.Surface.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_testScript : MonoBehaviour {

    //public InteractionGameEvent interactionEvent;
    public InteractionSupport interactionSupport;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            interactionSupport.StartInteraction();
        }
	}

    public void handleTestInteraction(GameObject go)
    {

    }
}
