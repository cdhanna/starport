using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smallgroup.Starport.Assets.Scripts.Inventory;
using UnityEngine.UI;

public class InventoryText : MonoBehaviour {
    private CharacterInventory inventory;
    public Text itemText;
	// Use this for initialization
	void Start () {
        inventory = new CharacterInventory();
        InventoryItem hammer = new InventoryItem("hammer", 1);
        inventory.addToInventory(hammer);
        inventory.addToInventory(hammer);

        InventoryItem gold = new InventoryItem("gold", 50);
        inventory.addToInventory(gold);

        foreach (KeyValuePair<string, InventoryItem> pair in inventory.inventory)
        {
            itemText.text += "Item: " + pair.Key + "\tQuantity: " + pair.Value.count + "\n";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
