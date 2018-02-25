using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;
using Dialog.Engine;
using Newtonsoft.Json;
using System.IO;

public class RuleButton : MonoBehaviour {
    public Button ruleButton;
    public String ruleText;
    private DialogRule rule;
    private Text conversationHistory;
    private DialogEngine dEngine;
    private DialogLoaderTest dialogLoader;
    
	// Use this for initialization
	void Start () {
        
	}

    public void Setup(Text conversationHistory, DialogRule rule, DialogEngine dEngine, DialogLoaderTest dialogLoader)
    {
        this.ruleButton = this.GetComponent<Button>();
        
        this.rule = rule;
        this.dEngine = dEngine;
        this.dialogLoader = dialogLoader;
        this.conversationHistory = conversationHistory;
        this.ruleButton.onClick.AddListener(Click);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Click()
    {
        
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
        dEngine.ExecuteRuleOutcomes(rule);
        dialogLoader.UpdateRuleOptions();
        dialogLoader.setTextFlag(true);
        //dialogLoader.scrollbar.value = 0;
    }

    }