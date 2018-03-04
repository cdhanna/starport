using Dialog.Engine;
using Smallgroup.Starport.Assets.Scripts.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBag : MonoBehaviour {

    public DialogAnchor Dialog;

    public string Name;

    public List<BagBoolElement> Flags = new List<BagBoolElement>();
    public List<BagIntElement> Ints = new List<BagIntElement>();
    public List<BagStringElement> Strs = new List<BagStringElement>();
    
    // Use this for initialization
    void Start () {
        var dEngine = Dialog.dEngine;
        Name = Name.ToLower();
        dEngine.AddAttribute(DialogAttribute.New(Name + ".flags", false, Flags).UpdateElements(dEngine));
        dEngine.AddAttribute(DialogAttribute.New(Name + ".ints", 0, Ints).UpdateElements(dEngine));
        dEngine.AddAttribute(DialogAttribute.New(Name + ".strs", "", Strs).UpdateElements(dEngine));
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
