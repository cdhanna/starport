using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorVizCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public void UpdateBoxes(int width, int height)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.gameObject.SendMessage(nameof(OnTriggerExit), other);

    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.SendMessage(nameof(OnTriggerEnter), other);
    }
}
