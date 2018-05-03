using Dialog.Engine;
using Smallgroup.Starport.Assets.Scripts.GameResources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerResources : MonoBehaviour {


    public List<GameResource> Resources;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameResource GetResource(string name)
    {
        return Resources.FirstOrDefault(res => res.ResourceType.Name.Equals(name));
    }

    public GameResource GetResource(GameResourceType type)
    {
        return Resources.FirstOrDefault(res => res.ResourceType == type);
    }

    public void AddDialog(string baseName, DialogEngine dEngine)
    {
        Resources.ForEach(res =>
        {
            dEngine.AddAttribute(DialogAttribute.New(baseName + "." + res.ResourceType.Name.Replace(" ", ".").ToLower(),
                v => res.Amount = v,
                () => res.Amount));
        });
    }

}
