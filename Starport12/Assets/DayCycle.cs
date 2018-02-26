using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour {
    public int numDays = 5;
    public RectTransform daysPanel;
    public Day dayTemplate;
    public Button advanceTurnButton;
    private Day[] days;
    public int currentDay;
    public bool isNight;
	// Use this for initialization
	void Start () {
        days = new Day[numDays];
		for (int i = 0; i < numDays; i++)
        {
            days[i] = Instantiate(dayTemplate);
            days[i].transform.parent = daysPanel.transform;
            days[i].transform.localPosition = new Vector3(i * 120, 0, 0);
            days[i].GetComponentInChildren<Text>().text = "Day " + (i + 1);
        }
        currentDay = 0;
        days[currentDay].GetComponentInChildren<Image>().color = UnityEngine.Color.yellow;
        isNight = false;
        this.advanceTurnButton.onClick.AddListener(advanceTurn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void advanceTurn()
    {
        if (isNight)
        {
            isNight = false;
            days[currentDay].GetComponentInChildren<Image>().color = UnityEngine.Color.grey;
            currentDay = ++currentDay % numDays;
            days[currentDay].GetComponentInChildren<Image>().color = UnityEngine.Color.yellow;
        }
        else
        {
            isNight = true;
            days[currentDay].GetComponentInChildren<Image>().color = UnityEngine.Color.blue;
        }
    }
}
