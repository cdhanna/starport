using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHelper : MonoBehaviour {

    public int TargetFPS = 60;

	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Application.targetFrameRate = TargetFPS;
	}
}
