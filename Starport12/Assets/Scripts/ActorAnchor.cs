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
        public bool UseMouse;
        public ControllerBinding Controller; 

        

        public SimpleActor Actor { get; set; } 
        public DefaultInputMech<SimpleActor> InputMech { get; set; }
        //public MapXY World { get; set; }

        public ActorAnchor()
        {
        }

        protected void Start()
        {
            var gob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Actor = new SimpleActor(World.Map, transform);
            gob.GetComponent<MeshRenderer>().material.color = Color;
            gob.transform.parent = transform;
            gob.transform.localScale *= .5f;


            World.Map.SetObjectPosition(StartPosition, Actor);

            if (UseMouse)
            {
                //var input = gameObject.AddComponent<MouseIntersector>();
                InputMech = new MouseIntersector();
                InputMech.Actor = Actor;
            } else
            {
                var input = new ControllerIntersector();
                //var input = gameObject.AddComponent<ControllerIntersector>();
                input.Actor = Actor;
                input.Binding = Controller;
                input.DebugColor = Color;
                InputMech = input;
            }
            InputMech.Init();
            transform.localPosition = World.Map.TransformCoordinateToWorld(World.Map.GetObjectPosition(Actor));
        }

        protected void Update()
        {
            Actor.Update();
            InputMech.Update();

            var coord = World.Map.GetObjectPosition(Actor);
            //transform.localPosition = World.Map.TransformCoordinateToWorld(coord);
            //var pos = World.Map.GetPosition(Actor);

            //var worldPos = new Vector3(pos.X, 0, pos.Y) + World.Map.CellOffset;
            //Debug.DrawLine(worldPos , worldPos + Vector3.up * 1, Color.red);
        }
    }
}
