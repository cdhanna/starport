using Smallgroup.Starport.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class CharacterSelectUI : MonoBehaviour {

    public Dropdown Dropdown;

    public event EventHandler<ActorAnchor> OnActorSelected = (s, a) => { };
    public Func<ActorAnchor, bool> ActorAllowed = (a) => true;
    private OptionData _deselectOption = new OptionData("None");
    private List<OptionData> _options;
    private ActorAnchor _actor;
    private class SubOptionData : OptionData
    {
        public ActorAnchor Actor;
        public SubOptionData(ActorAnchor actor)
            : base(actor.Character.DisplayName)
        {
            Actor = actor;
        }
    }

	// Use this for initialization
	void Start () {
        Dropdown.ClearOptions();


        var allActors = GameObject.FindObjectsOfType<ActorAnchor>();

        _options = allActors
            .Where(a => ActorAllowed(a) == true)
            .Select(a => new SubOptionData(a))
            .Cast<OptionData>()
            .ToList();
        _options.Insert(0, _deselectOption);

        Dropdown.AddOptions(_options);

        //Dropdown.va
        Dropdown.onValueChanged.AddListener(new UnityAction<int>(OnValueChange));
        SelectActor(_actor);
       // Dropdown.on
    }


	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnValueChange(int index)
    {
        var option = Dropdown.options[index];
        if (option == _deselectOption )
        {
            
            OnActorSelected(this, null);
            return;
        } else
        {
            var actorOption = option as SubOptionData;
           // if (actorOption?.Actor != _actor)
            {
                _actor = actorOption.Actor;
                OnActorSelected(this, actorOption?.Actor);

            }
        }
    }

    public void SelectActor(ActorAnchor actor)
    {
        //if (_actor == actor) return;
        _actor = actor;
        if (_options == null) return;

        if (actor == null)
        {
            Dropdown.value = 0;
        } else
        {
            Dropdown.value = _options.FindIndex(data => (data as SubOptionData)?.Actor == actor);
        }
    }

   
}
