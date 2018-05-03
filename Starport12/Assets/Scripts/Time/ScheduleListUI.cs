using Smallgroup.Starport.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduleListUI : MonoBehaviour {

    public ActorRowUI ActorRowGameObjectPrefab;
    public GameObject RowParent;

    public bool IsOpen;
    private Animator _animator;
    private Dictionary<ActorAnchor, ActorRowUI> _actor2UI = new Dictionary<ActorAnchor, ActorRowUI>();

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
        _animator.SetBool("isOpen", IsOpen);

    }

    public void Open()
    {
        IsOpen = true;

        _actor2UI.Values.ToList().ForEach(ui => Destroy(ui.gameObject));
        _actor2UI.Clear();
        var all = GameObject.FindObjectsOfType<ActorAnchor>()
            .Where(a =>
            {
                return a.Character.IsCommandable == true;
            })
            .ToList();
        all.ForEach(actor =>
        {
            var ui = Instantiate(ActorRowGameObjectPrefab, RowParent.transform);
            ui.Actor = actor;
            _actor2UI.Add(actor, ui);
        });

    }

    public void Close()
    {
        IsOpen = false;
    }
}
