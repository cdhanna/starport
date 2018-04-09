using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Surface.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InteractionStart(Interaction[] availableInteractions)
    {
        GameObject clickedObject = availableInteractions[0].gameObject;
        ActorAnchor aa = clickedObject.GetComponent<ActorAnchor>();
    }
}
