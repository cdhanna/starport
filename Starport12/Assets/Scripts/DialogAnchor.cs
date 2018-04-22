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
using Smallgroup.Starport.Assets.Scripts.Dialogue;
using System.Text;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using Smallgroup.Starport.Assets.Scripts.MapSelect;
using Smallgroup.Starport.Assets.Scripts.JobSystem;

public class DialogAnchor : MonoBehaviour {


    public TextAsset[] DialogFiles;

    public string[] jsonFiles = new string[] {
        "Assets\\sample json data\\Phieneas_the_Collector_Dialog.json" // default file
    };
    public DialogUI convotemplate;
    public DialogEngine dEngine = new DialogEngine()
        .AddHandler(new BagBoolHandler())
        .AddHandler(new BagIntHandler())
        .AddHandler(new BagStringHandler())
        //.AddHandler(new BagVec2Handler())
        .AddHandler(new ConditionSetEvalHandler());

    public ObjectMoveTaskType moveTaskType;

    [Header("debug only. Dont edit")]
    public string[] handlers;
    public string[] loadedRuleNames;
    public string[] attributes;
    public bool ConversationFlag;

    public bool IsDialogOpen
    {
        get { return dialogInstance != null; }
    }

    private DialogUI dialogInstance;
    private List<DialogRule> allRules = new List<DialogRule>();
    private ActorAnchor _speaker, _target;


    private int _gayHack = 2;

    // Use this for initialization
    void Start () {

        dEngine.AddAttribute(DialogAttribute.New("conversation", v => ConversationFlag = v, () => ConversationFlag));
        dEngine.AddTransform("dialog.target", () => _target.Character.CodedName);
        dEngine.AddTransform("dialog.speaker", () => _speaker.Character.CodedName);

        var moveFunc = new ObjectFunctionDialogAttribute("tasks.move", args =>
        {
            var xPos = (int)args["x"];
            var yPos = (int)args["y"];
            var item = (string)args["item"];
            var assignee = (string)args["assignee"];

            var itemValue = GameObject.FindObjectsOfType<InteractionSupport>().First(o => o.name.Equals(item));
            var gotoValue = new SelectionResult()
            {
                Height = 0,
                Width = 0,
                OverlappingObjects = new List<GameObject>(),
                World = FindObjectOfType<WorldAnchor>(),
                X = xPos,
                Y = yPos
            };

            var instance = moveTaskType.CreateInstance();

            instance.SetValue(moveTaskType.ObjectToMoveParameter, itemValue);
            instance.SetValue(moveTaskType.LocationToGoParameter, gotoValue);
            instance.SetValue(moveTaskType.Assignments[0], FindObjectsOfType<ActorAnchor>().FirstOrDefault(a => a.Character.DisplayName.Equals(assignee)));
            FindObjectOfType<JobHandler>().AddTask(instance, true);


        }, new Dictionary<string, object>()
        {
            { "item", "thing" },
            { "x", -1 },
            { "y", -1 },
            { "assignee", "noone" }
        });
        dEngine.AddAttribute(moveFunc);
        //dEngine.AddAttribute(DialogAttribute.New("dialog.target.name", v => { }, () => _target.Name));

        //var gotoFunc = new ObjectFunctionDialogAttribute("dialog.target.funcs.goto", args =>
        //{
        //    var xPosition = (int)args["x"];
        //    var yPosition = (int)args["y"];
        //    _target.AddCommand(new GotoCommand() { Target = new GridXY(xPosition, yPosition) });
        //}, new Dictionary<string, object>() {
        //        { "x", -1 },
        //        { "y", -1 }
        //    });
        //dEngine.AddAttribute(gotoFunc);

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
        handlers = dEngine.GetHandlerNames;
        
        if (_gayHack == 0)
        {
            // load up all rules from all files
            // defer loading of rules until next step
           
            foreach (var file in DialogFiles)
            {
                var json = Encoding.UTF8.GetString(file.bytes);
                var bundle = JsonConvert.DeserializeObject<DialogBundle>(json);
                allRules.AddRange(bundle.Rules);

                foreach (var set in bundle.ConditionSets)
                {
                    dEngine.AddConditionSet(set);
                }

                var rules = bundle.Rules.ToList();
                foreach (var rule in rules) {
                    var refs = dEngine.ExtractReferencesFromRule(rule);
                    dEngine.AddRule(rule);
                }
            }

            loadedRuleNames = allRules.Select(r => r.Name).ToArray();

            var listing = dEngine.GetAttributeNames().ToList();
            listing.Sort();
            attributes = listing.ToArray();
           
            _gayHack = -1;
        } else if (_gayHack > 0)
        {
            _gayHack -= 1;
        }

	}

    IEnumerator CloseDialogInSeconds(float seconds)
    {
        ConversationFlag = true;
        yield return new WaitForSeconds(seconds);
        CloseDialog();
    }

    public void OpenDialog(ActorAnchor speaker, ActorAnchor target)
    {
        if (IsDialogOpen)
        {
            throw new Exception("Dialog is already open");
        }
        _speaker = speaker;
        _target = target;
        ConversationFlag = true;
        dialogInstance = Instantiate(convotemplate, GameObject.FindObjectOfType<Canvas>().transform);
        dialogInstance.SetTargetSpeaker(target.Character);
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
