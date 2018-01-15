using UnityEngine;
using System.Collections;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using UnityEngine.Experimental.UIElements;

public class MouseIntersector : DefaultInputMech<SimpleActor>
{

    private Plane _ground;
    private Vector3 _lastHit;

    private GameObject _mouseIndicator;

    // Use this for initialization
    public override void Init()
    {

        _ground = new Plane(Vector3.up, 0);

        _mouseIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _mouseIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        _mouseIndicator.transform.localScale = Vector3.one * .2f;
    }

    // Update is called once per frame
    public override void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // plane.Raycast returns the distance from the ray start to the hit point
        var distance = 0f;
        if (_ground.Raycast(ray, out distance))
        {
            // some point of the plane was hit - get its coordinates
            var hitPoint = ray.GetPoint(distance);
            _lastHit = hitPoint;
            _mouseIndicator.transform.position = hitPoint;
            // use the hitPoint to aim your cannon
        }

        if (Input.GetMouseButtonDown(0))
        {
            var clickedCoord = World.Map.TransformWorldToCoordinate(_lastHit);

            Debug.Log("CLICKED ON " + clickedCoord + " from " + _lastHit.x + "," + _lastHit.z);

            if (World.Map.CoordinateExists(clickedCoord))
            {
                Debug.Log("SUBMITTING COMMAND");
                Actor.ClearCommands();
                Actor.AddCommand(new GotoCommand(clickedCoord));
            }
        }
    }
}
