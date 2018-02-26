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
    public DialogPlayer player2;

    public DialogEngine dEngine;
    public Text conversationHistory;
    public Scrollbar scrollbar;
    public RuleButton ruleButtonTemplate;
    public RectTransform buttonPanel;
    
    private RuleButton[] buttons;
    private DialogRule[] rules;
    private bool textAdded;
    public bool conversationFlag = false;


    // Use this for initialization
    void Start () {
        
        ObjectDialogAttribute playerHealth = new ObjectDialogAttribute(player1, "player", "health");
        ObjectDialogAttribute playerRespect = new ObjectDialogAttribute(player1, "player", "respect");

        ObjectDialogAttribute merchant = new ObjectDialogAttribute(player2, "Phineas.the.Collector", "merchant");
        ObjectDialogAttribute criminal = new ObjectDialogAttribute(player2, "Phineas.the.Collector", "criminal");
        ObjectDialogAttribute seeker = new ObjectDialogAttribute(player2, "Phineas.the.Collector", "seeker");
        ObjectDialogAttribute trust = new ObjectDialogAttribute(player2, "Phineas.the.Collector", "trust");
        ObjectDialogAttribute collection = new ObjectDialogAttribute(player2, "Phineas.the.Collector", "collection");
        var conversation = GlobalDialogAttribute.New("conversation", value => conversationFlag = value, () => conversationFlag);


        player1.health = 13;
        player1.respect = 15;

   
        String jsonPath = "Assets\\sample json data\\Phieneas_the_Collector_Dialog.json";

        var json = File.ReadAllText(jsonPath);
        rules = JsonConvert.DeserializeObject<DialogRule[]>(json);

        dEngine = new DialogEngine();
        dEngine.AddAttribute(playerHealth);
        dEngine.AddAttribute(playerRespect);
        dEngine.AddAttribute(merchant);
        dEngine.AddAttribute(collection);
        dEngine.AddAttribute(criminal);
        dEngine.AddAttribute(seeker);
        dEngine.AddAttribute(trust);
        dEngine.AddAttribute(conversation);

        for (int i = 0; i < rules.Length; i++)
        {
            dEngine.AddRule(rules[i]);
        }

        UpdateRuleOptions();
        
    }
    public void setTextFlag(bool val)
    {
        textAdded = val;
    }
    public void CloseDialog()
    {
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (textAdded)
        {
            scrollbar.value = 0;
            textAdded = false;
        }
    }

    public void UpdateRuleOptions()
    {

        List<DialogRule> validRules = dEngine.GetAllValidDialog();

        if (buttons != null) { 
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }
        
        buttons = new RuleButton[validRules.Count];

        for (int i = 0; i < validRules.Count; i++)
        {
            buttons[i] = Instantiate(ruleButtonTemplate);
            buttons[i].transform.parent = buttonPanel.transform;

            // fix this to work
            buttons[i].transform.localPosition = new Vector3(-380, 60 + i * -40, 0);
            buttons[i].Setup(conversationHistory, validRules[i], dEngine, this);
            buttons[i].ruleButton.GetComponentInChildren<Text>().text = validRules[i].DisplayAs;

        }
    }


}
