using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Surface.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelUI : MonoBehaviour
{

    public ActorAnchor Actor;
    private ActorAnchor _lastActor;

    public Image CharacterImage;
    public Text GoldText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Actor != _lastActor)
        {
            RefreshActor();
        }
        if (Actor != null && Actor.Resources != null)
        {
            var gold = Actor.Resources.GetResource("Gold");
            GoldText.text = (gold == null) ? "-" : gold.Amount.ToString();
        }

        _lastActor = Actor;
    }

    void RefreshActor()
    {
        CharacterImage.overrideSprite = Actor.Character.Icon;
    }

    public void InteractionStart(Interaction[] availableInteractions)
    {
        GameObject clickedObject = availableInteractions[0].gameObject;
        ActorAnchor aa = clickedObject.GetComponent<ActorAnchor>();
    }
}