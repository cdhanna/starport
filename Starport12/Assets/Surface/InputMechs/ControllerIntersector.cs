using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.InputMechs
{
    public class ControllerIntersector : DefaultInputMech<SimpleActor>
    {
        
        private Vector3 _pos;

        private GameObject _indicator;

        public ControllerBinding Binding { get; set; }
        public Color DebugColor { get; set; }

        // Use this for initialization
        public override void Init()
        {
            
            _indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _indicator.GetComponent<MeshRenderer>().material.color = DebugColor;
            _indicator.transform.localScale = Vector3.one * .2f;
        }

        // Update is called once per frame
        public override void Update()
        {
            
            var x = Input.GetAxis(Binding.X_AXIS);
            var y = Input.GetAxis(Binding.Y_AXIS);
            _pos.x += x * .5f;
            _pos.z -= y * .5f;




            //var ray = Camera.main.ScreenPointToRay(_pos);
            // plane.Raycast returns the distance from the ray start to the hit point
            //var distance = 0f;
            //if (_ground.Raycast(ray, out distance))
            //{
            //    // some point of the plane was hit - get its coordinates
            //    var hitPoint = ray.GetPoint(distance);
            //    _lastHit = hitPoint;
            _indicator.transform.position = _pos;
            //    // use the hitPoint to aim your cannon
            //}
            //http://wiki.unity3d.com/index.php?title=Xbox360Controller
            if (Input.GetButtonDown(Binding.A_BUTTON))
            {
                var clickedCoord = World.Map.TransformWorldToCoordinate(_pos);

                Debug.Log("CLICKED ON " + clickedCoord + " from " + _pos.x + "," + _pos.z);

                if (World.Map.CoordinateExists(clickedCoord))
                {
                    Debug.Log("SUBMITTING COMMAND");
                    Actor.ClearCommands();
                    Actor.AddCommand(new GotoCommand(clickedCoord));
                }
            }
        }

    }
}
