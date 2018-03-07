using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smallgroup.Starport.Assets.Scripts.Inventory;
using UnityEngine.UI;

public class InventoryText : MonoBehaviour {
    private CharacterInventory inventory;
    public Text itemText;
    public Button addHammerButton, removeHammerButton;
    public InputField itemInputField;
	// Use this for initialization
	void Start () {
        inventory = new CharacterInventory();
        
     

        InventoryItem gold = new InventoryItem("gold", 50);
        inventory.addToInventory(gold);

        refresh();
        this.addHammerButton.onClick.AddListener(addHammer);
        this.removeHammerButton.onClick.AddListener(removeHammer);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void refresh()
    {
        itemText.text = "";
        foreach (KeyValuePair<string, InventoryItem> pair in inventory.inventory)
        {
            itemText.text += "Item: " + pair.Key + "\tQuantity: " + pair.Value.count + "\n";
        }
    }

    public void addHammer()
    {
        string itemName = itemInputField.text;
        InventoryItem hammer = new InventoryItem(itemName, 1);
        inventory.addToInventory(hammer);
        refresh();
    }

    public void removeHammer()
    {
        string itemName = itemInputField.text;
        
        inventory.removeFromInventory(itemName);
        refresh();
    }
}
