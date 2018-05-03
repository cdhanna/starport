using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {

    public float xThresholdToScroll = .1f;
    public float yThresholdToScroll = .1f;

    public float xSpeed = .1f;
    public float ySpeed = .1f;
    public float height = 5;
    public Camera Target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        var scrollDir = Math.Sign(Input.mouseScrollDelta.y);
        height -= scrollDir;

        var mousePos = Input.mousePosition;

        var leftThreshold = Screen.width * xThresholdToScroll;
        var rightThreshold = Screen.width - leftThreshold;

        var topThreshold = Screen.height * yThresholdToScroll;
        var lowThreshold = Screen.height - topThreshold;

        var leftPull = Math.Min(1, Math.Max(0, leftThreshold - mousePos.x) / leftThreshold);
        var rightPull = Math.Min(1, Math.Max(0, mousePos.x - rightThreshold) / leftThreshold);
        var topPull = Math.Min(1, Math.Max(0, topThreshold - mousePos.y) / topThreshold);
        var lowpUllPull = Math.Min(1, Math.Max(0, mousePos.y - lowThreshold) / topThreshold);
        var xDiff = 0f;
        var yDiff = 0f;
        xDiff -= leftPull * xSpeed * height;
        xDiff += rightPull * xSpeed * height;
        yDiff -= topPull * ySpeed * height;
        yDiff += lowpUllPull * ySpeed * height;

        var pos = Target.transform.localPosition;
        pos += new Vector3(xDiff, 0, yDiff);
        pos = new Vector3(pos.x, height, pos.z);

        Target.transform.localPosition = pos;

	}
}
