using Dialog;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogLoaderTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var json = File.ReadAllText("Assets\\sample json data\\sampleDialog1.json");
        var rules = JsonConvert.DeserializeObject<DialogRule[]>(json);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
