using Dialog;
using Dialog.Engine;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Scripts;

public class DialogAnchor : MonoBehaviour {
    
    public string[] jsonFiles = new string[] {
        "Assets\\sample json data\\Phieneas_the_Collector_Dialog.json" // default file
    };
    public DialogUI convotemplate;
    public DialogEngine dEngine = new DialogEngine();

    [Header("debug only. Dont edit")]
    public string[] loadedRuleNames;
    public string[] attributes;
    public bool ConversationFlag;

    public bool IsDialogOpen
    {
        get { return dialogInstance != null; }
    }

    private DialogUI dialogInstance;
    private List<DialogRule> allRules = new List<DialogRule>();

    // Use this for initialization
    void Start () {

        dEngine.AddAttribute(GlobalDialogAttribute.New("conversation", v => ConversationFlag = v, () => ConversationFlag));

        // load up all rules from all files
        foreach (var file in jsonFiles)
        {
            var json = File.ReadAllText(file);
            var rules = JsonConvert.DeserializeObject<DialogRule[]>(json).ToList();
            allRules.AddRange(rules);

            rules.ForEach(r => dEngine.AddRule(r));
        }

        loadedRuleNames = allRules.Select(r => r.Name).ToArray();
       
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && IsDialogOpen)
        {
            CloseDialog();
        }
        if (IsDialogOpen && !ConversationFlag)
        {
            StartCoroutine(CloseDialogInSeconds(1));
        }
	}

    IEnumerator CloseDialogInSeconds(float seconds)
    {
        ConversationFlag = true;
        yield return new WaitForSeconds(seconds);
        CloseDialog();
    }

    public void OpenDialog()
    {
        if (IsDialogOpen)
        {
            throw new Exception("Dialog is already open");
        }
        ConversationFlag = true;
        dialogInstance = Instantiate(convotemplate);
        dialogInstance.UpdateRuleOptions(dEngine);
        GameObject.FindObjectsOfType<ActorAnchor>().ToList().ForEach(anchor => anchor.InputMech.Ignore = true);

    }

    public void CloseDialog()
    {
        if (!IsDialogOpen)
        {
            throw new Exception("Dialog is not open");
        }
        ConversationFlag = false;

        dialogInstance.CloseDialog();
        dialogInstance = null;

        GameObject.FindObjectsOfType<ActorAnchor>().ToList().ForEach(anchor => anchor.InputMech.Ignore = false);

    }
}
