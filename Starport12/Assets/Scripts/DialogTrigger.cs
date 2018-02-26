using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    public DialogLoaderTest convotemplate;
    private DialogLoaderTest dlt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
        {
            dlt = Instantiate(convotemplate);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dlt.CloseDialog();
        }
    }
}
