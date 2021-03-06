﻿using UnityEngine;
using System.Collections;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using UnityEngine.Experimental.UIElements;
using Smallgroup.Starport.Assets.Scripts;
using UnityEngine.EventSystems;

public class MouseIntersector : DefaultInputMech<SimpleActor>
{

    private Plane _ground;
    private Vector3 _lastHit;

    private GameObject _mouseIndicator;

    private HoverableObject _currentlyHovering;




    // Use this for initialization
    public override void Init()
    {

        _ground = new Plane(Vector3.up, 0);

        _mouseIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(_mouseIndicator.GetComponent<SphereCollider>());
        _mouseIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        _mouseIndicator.transform.localScale = Vector3.one * .2f;
    }

    // Update is called once per frame
    protected override InputMoment GetMoment()
    {
        if (Ignore) return new InputMoment();

        var moment = new InputMoment();

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // plane.Raycast returns the distance from the ray start to the hit point
        var distance = 0f;

        CheckForHover(ray);
        if (_ground.Raycast(ray, out distance))
        {
            // some point of the plane was hit - get its coordinates
            var hitPoint = ray.GetPoint(distance);
            _lastHit = hitPoint;
            _mouseIndicator.transform.position = hitPoint;
            // use the hitPoint to aim your cannon
        }


        moment.Point = new Vector2(_lastHit.x, _lastHit.z);
        moment.Coord = World.Map.TransformWorldToCoordinate(_lastHit);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (_currentlyHovering != null)
            {
                moment.InteractableObject = _currentlyHovering.GetComponent<InteractionSupport>();

            }
           

            if (Input.GetMouseButton(1))
            {
                moment.Secondary = true;
            }

            if (Input.GetMouseButton(0))
            {
                var clickedCoord = World.Map.TransformWorldToCoordinate(_lastHit);
                moment.Primary = true;
                Debug.Log("CLICKED ON " + clickedCoord + " from " + _lastHit.x + "," + _lastHit.z);

                //if (_currentlyHovering != null)
                //{
                //    var selectable = _currentlyHovering.GetComponent<SelectableObject>();
                //    if (selectable != null)
                //    {
                //        // TODO. think about how better to handle these comamnds
                //        //selectable.Select();

                //    }

                //    var interactable = _currentlyHovering.GetComponent<InteractionSupport>();
                //    if (interactable != null)
                //    {
                //        interactable.StartInteraction();
                //    }


                //    var otherActor = _currentlyHovering.GetComponent<ActorAnchor>();
                //    if (otherActor != null)
                //    {
                //        Actor.ClearCommands();
                //        Actor.AddCommand(new GotoCommand(otherActor.Actor.Coordinate, _lastHit));
                //        Actor.AddCommand(new OpenDialogCommand(otherActor.Actor));
                //    }

                //} else
                //{


                //    if (World.Map.CoordinateExists(clickedCoord))
                //    {
                //        Debug.Log("SUBMITTING COMMAND");
                //        Actor.ClearCommands();
                //        Actor.AddCommand(new GotoCommand(clickedCoord, _lastHit));
                //    }
                //}
            }
        }

        return moment;
    }

    private void CheckForHover(Ray ray)
    {

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
           
            var hoverable = hit.transform.GetComponentInParent<HoverableObject>();
            if (hoverable != null)
            {
                if (hoverable != _currentlyHovering && _currentlyHovering != null)
                {
                    _currentlyHovering.StopHover();
                }
                
                hoverable.StartHover();
                _currentlyHovering = hoverable;
            } else if (_currentlyHovering != null)
            {

                _currentlyHovering.StopHover();
                _currentlyHovering = null;
            }
        } else if (_currentlyHovering != null)
        {
            _currentlyHovering.StopHover();
            _currentlyHovering = null;
        }
    }
}
