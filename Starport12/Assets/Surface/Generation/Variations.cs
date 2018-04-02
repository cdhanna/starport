using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variations : MonoBehaviour {


    static int counter = 0;

    private void Awake()
    {

        var pick = counter % transform.childCount;
        counter += 1;
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(pick == i);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
