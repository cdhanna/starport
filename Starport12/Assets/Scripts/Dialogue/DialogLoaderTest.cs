using Dialog;
using Dialog.Engine;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogLoaderTest : MonoBehaviour {

    public DialogPlayer player1;
    public DialogEngine dEngine;
    public Text conversationHistory;
    public Scrollbar scrollbar;

	// Use this for initialization
	void Start () {
        ObjectDialogAttribute playerHealth = new ObjectDialogAttribute(player1, "player", "health");
        ObjectDialogAttribute playerRespect = new ObjectDialogAttribute(player1, "player", "respect");

        
        player1.health = 51;
        player1.respect = 15;

        var json = File.ReadAllText("Assets\\sample json data\\wh_rule1.json");
        var rules = JsonConvert.DeserializeObject<DialogRule[]>(json);

        dEngine = new DialogEngine();
        dEngine.AddAttribute(playerHealth);
        dEngine.AddAttribute(playerRespect);

        for (int i = 0; i < rules.Length; i++)
        {
            dEngine.AddRule(rules[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            var rule = dEngine.GetBestValidDialog();
            for (int i = 0; i < rule.Dialog.Length; i++)
            {
                String speaker = rule.Dialog[i].Speaker;
                String content = rule.Dialog[i].Content;
                String red = "<color=#ff0000ff>";
                String green = "<color=#008000ff>";

                // assumes only 2 speakers. also doesn't account for the order changing throughout the life of the convo. 
                String speakerColor = i % 2 == 0 ? red : green;

                conversationHistory.text += speakerColor + speaker + "</color>: " + content + '\n';
            }

            scrollbar.value = 0; // hack for getting the scroll bar to auto adjust to the end
        }
	}
}
