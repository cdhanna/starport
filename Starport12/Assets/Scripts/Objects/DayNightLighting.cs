using Smallgroup.Starport.Assets.Scripts.Time;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class DayNightLighting : MonoBehaviour {

    private Light Light;

    public Color NightColor;
    public Color DayColor;

	// Use this for initialization
	void Start () {
        Light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetForTimeOfDay(Calendar calendar)
    {
        if (calendar.IsNight())
        {
            Light.color = NightColor;
        } else
        {
            Light.color = DayColor;
        }

    }
}
