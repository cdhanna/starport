using Smallgroup.Starport.Assets.Surface.Generation.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHellp : SuperRule {

    public GameObject ToSpawn;

    public override DefaultCtxRule Rule
    {
        get
        {
            return new RuleSpawn()
            {
                Template = ToSpawn
            };
        }
    }

    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
