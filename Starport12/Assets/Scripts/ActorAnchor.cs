using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface.InputMechs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Smallgroup.Starport.Assets.Scripts
{
    public class ActorAnchor : MonoBehaviour
    {
        public Color Color;
        //public GridXY StartPosition;
        public bool UseMouse;
        public ControllerBinding Controller;

        public MapZone SpawnZone;

        public WorldAnchor World;

        private DialogAnchor DialogAnchor;

        public SimpleActor Actor;
        public DefaultInputMech<SimpleActor> InputMech { get; set; }

        private Material standardMat;
        private GameObject gob;

        private bool _secondPass = false;


        //public MapXY World { get; set; }

        public ActorAnchor()
        {
        }

        private void Awake()
        {
            if (!_secondPass)
            {
                GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        protected void Start()
        {
            DialogAnchor = World.DialogAnchor;
            Actor.Setup(World.Map, this, transform);
            Actor.InitDialogAttributes(DialogAnchor);

           

            gob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //Actor = new SimpleActor(World.Map, transform);
            gob.GetComponent<MeshRenderer>().material.color = Color;
            standardMat = gob.GetComponent<MeshRenderer>().material;
            gob.transform.parent = transform;
            gob.transform.localScale *= .5f;
            gob.transform.localPosition = Vector3.zero;


            var possibleCells = World.Map.Coordinates.Select(xy => World.Map.GetCell(xy)).Where(c => {
                var proc = World.Map.Handlers.Zones.Process(c);
                return proc == SpawnZone;
                }).ToList();

            float n = UnityEngine.Random.Range(0, possibleCells.Count);
            var startCell = possibleCells[(int)(Math.Round(n, 1))];
            var startCoord = World.Map.GetCoordinate(startCell);
            World.Map.SetObjectPosition(startCoord, Actor);

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
            if (_secondPass)
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }
            _secondPass = true;
            if (DialogAnchor==null || !DialogAnchor.ConversationFlag)
            {
                Actor.Update();
                if (InputMech != null)
                {
                    InputMech.Update();
                }

            }



            var coord = World.Map.GetObjectPosition(Actor);
            //transform.localPosition = World.Map.TransformCoordinateToWorld(coord);
            //var pos = World.Map.GetPosition(Actor);

            //var worldPos = new Vector3(pos.X, 0, pos.Y) + World.Map.CellOffset;
            //Debug.DrawLine(worldPos , worldPos + Vector3.up * 1, Color.red);
        }

        public void IssueGotoCommand(GameObject target)
        {
            Actor.ClearCommands();
            Actor.AddCommand(new GotoCommand() { Target = World.Map.TransformWorldToCoordinate(target.transform.position), Actual = target.transform.position });
        }

        public void SetColor(Color color)
        {
            gob.GetComponent<MeshRenderer>().material.color = color;
        }
        public void ResetColor()
        {
            gob.GetComponent<MeshRenderer>().material.color = Color;
            
        }
        

    }
}
