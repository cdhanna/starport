using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Scripts.Time;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorRowUI : MonoBehaviour {

    public ActorAnchor Actor;
    private ActorAnchor _lastActor;


    public Text NameText;
    public Image CharacterImage;
    public Text StatusText;


    private GameObject _resPanel;
    private GameObject _resInstanceTemplate;

	// Use this for initialization
	void Start () {
        _resPanel = transform.Find("resourcePanel").gameObject;
        _resInstanceTemplate = _resPanel.transform.Find("template").gameObject;
        _resInstanceTemplate.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Actor != null)
        {
            if (Actor != _lastActor)
            {

                NameText.text = Actor.Character.DisplayName;
                CharacterImage.overrideSprite = Actor.Character.Icon;

                Actor.Character.ResourceAbilities.ForEach(res =>
                {
                    var ui = Instantiate(_resInstanceTemplate, _resPanel.transform);
                    ui.transform.Find("Image").gameObject.GetComponent<Image>().overrideSprite = res.ResourceType.Icon;
                    ui.transform.Find("count").gameObject.GetComponent<Text>().text = " " + res.Amount;
                    ui.SetActive(true);
                });

            }
            //StatusText.text = 
            //AboutText.text = "character stats go here";
            StatusText.text = Actor.Schedule.GetActivityNow() == DefaultTimePledge.Idle ? "Free" : "Busy";
        }

        _lastActor = Actor;
	}
}
