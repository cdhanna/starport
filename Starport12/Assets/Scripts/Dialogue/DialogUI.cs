using Dialog;
using Dialog.Engine;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour {

    //public DialogPlayer player1;
    //public DialogPlayer player2;

    public Text conversationHistory;
    public Scrollbar scrollbar;
    public RuleButton ruleButtonTemplate;
    public RectTransform buttonPanel;
    
    private List<RuleButton> buttons = new List<RuleButton>();
    //private DialogRule[] rules;
    private bool textAdded;
    public bool conversationFlag = false;


    // Use this for initialization
    void Start () {

        
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

    public void UpdateRuleOptions(DialogEngine dEngine)
    {
        try
        {
            var codes = dEngine.GetAttributeValueCodes();
        } catch (Exception ex)
        {
            throw ex;
        }
        List<DialogRule> validRules = dEngine.GetAllValidDialog();

        
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
        buttonPanel.sizeDelta = new Vector2(300, 50 * validRules.Count);
        
        for (int i = 0; i < validRules.Count; i++)
        {
            var button = Instantiate(ruleButtonTemplate);
            button.transform.parent = buttonPanel.transform;

            // fix this to work
            var rectTransform = button.GetComponent<RectTransform>();
            var parentRect = buttonPanel.transform.GetComponent<RectTransform>();
            //rectTransform.anchorMin = new Vector2(0, 0);
            //rectTransform.anchoredPosition = new Vector2(0, -1);

            rectTransform.anchoredPosition = new Vector2(
                -parentRect.rect.width/2 + 20,
                 parentRect.rect.height/4 -i*50);

            //button.transform.localPosition = new Vector3(-380, 60 + i * -40, 0);
            button.Setup(conversationHistory, validRules[i], dEngine, this);
            button.ruleButton.GetComponentInChildren<Text>().text = validRules[i].DisplayAs;

            buttons.Add(button);
        }
    }


}
