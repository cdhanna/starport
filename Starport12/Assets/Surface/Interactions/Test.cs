using Smallgroup.Starport.Assets.Surface.Interactions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleSearch(GameObject obj)
    {
        Debug.Log("Searching for " + obj.name);
    }

    public void HandleClick(Interaction[] interactions)
    {
        Debug.Log("available interactions..." + interactions.Length);
        interactions.ToList().ForEach(i => Debug.Log("  " + i.name + ":" + i.GetDisplayName()));
        interactions.First().Invoke();
    }
}
