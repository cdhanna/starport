using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        string txt = System.IO.File.ReadAllText(@"C:\Users\Will\Proj\starport\wh_texts\dummy.txt");
        text.text = txt;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
