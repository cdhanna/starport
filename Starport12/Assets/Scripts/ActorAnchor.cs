using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.InputMechs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    class ActorAnchor : MonoBehaviour
    {
        public Color Color;
        public GridXY StartPosition;
        public ControllerBinding Controller;

        public SimpleActor Actor { get; set; }
        //public MapXY World { get; set; }

        public ActorAnchor()
        {
        }

        protected void Start()
        {
            Actor = new SimpleActor(World.Map);
            var gob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            gob.GetComponent<MeshRenderer>().material.color = Color;
            gob.transform.parent = transform;


            World.Map.SetObjectPosition(StartPosition, Actor);

            var input = gob.AddComponent<ControllerIntersector>();
            input.Actor = Actor;
            input.Binding = Controller;
            input.DebugColor = Color;
            

            //var input = gob.AddComponent<SimpleKeyboardInput>();
            //Actor.InputMech = input;
            //input.Actor = Actor;
        }

        protected void Update()
        {
            Actor.Update();

            var coord = World.Map.GetObjectPosition(Actor);
            transform.localPosition = World.Map.TransformCoordinateToWorld(coord);
            //var pos = World.Map.GetPosition(Actor);

            //var worldPos = new Vector3(pos.X, 0, pos.Y) + World.Map.CellOffset;
            //Debug.DrawLine(worldPos , worldPos + Vector3.up * 1, Color.red);
        }
    }
}
