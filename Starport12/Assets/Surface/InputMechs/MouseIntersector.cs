using UnityEngine;
using System.Collections;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using UnityEngine.Experimental.UIElements;

public class MouseIntersector : MonoBehaviour, InputMechanism<SimpleActor>
{

    private Plane _ground;
    private Vector3 _lastHit;


    // Use this for initialization
    void Start()
    {

        _ground = new Plane(Vector3.up, 0);


    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // plane.Raycast returns the distance from the ray start to the hit point
        var distance = 0f;
        if (_ground.Raycast(ray, out distance))
        {
            // some point of the plane was hit - get its coordinates
            var hitPoint = ray.GetPoint(distance);
            _lastHit = hitPoint;
            // use the hitPoint to aim your cannon
        }

        if (Input.GetMouseButtonDown(0))
        {
            var clickedCoord = World.Map.TransformWorldToCoordinate(_lastHit);
        }
    }
}
