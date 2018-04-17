using Smallgroup.Starport.Assets.Core.Generation;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Scripts.Characters.Commands;
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
        public MapSelection BoxSelector;

        public WorldAnchor World;

        private DialogAnchor DialogAnchor;

        public SimpleActor Actor;
        public DefaultInputMech<SimpleActor> InputMech { get; set; }


        private Material standardMat;
        private GameObject gob;

        private bool _secondPass = false;

        private InteractionBasedCommand currInteractionCommand;
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

            BoxSelector = Instantiate(BoxSelector, transform);
            BoxSelector.World = World;
            BoxSelector.name = "ActorSelector";


            gob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //Actor = new SimpleActor(World.Map, transform);
            gob.GetComponent<MeshRenderer>().material.color = Color;
            standardMat = gob.GetComponent<MeshRenderer>().material;
            gob.transform.parent = transform;
            gob.transform.localScale *= .5f;
            gob.transform.localPosition = Vector3.zero;
            gob.layer = gameObject.layer;

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
                InputMech = new SimpleInputMech();
                //var input = gameObject.AddComponent<ControllerIntersector>();
                
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


            currInteractionCommand = Actor.ActiveCommand as InteractionBasedCommand;
            if (InputMech != null)
            {
                if (InputMech.NewPrimary && currInteractionCommand == null)
                {
                    // goto command.
                    Actor.ClearCommands();
                    Actor.AddCommand(new GotoCommand(InputMech.Now.Coord, InputMech.Now.Point));
                }
                if (InputMech.NewSecondary)
                {
                    if (currInteractionCommand == null && InputMech.Now.InteractableObject == null)
                    {
                        IssueBuildCommand();
                    }
                    if (currInteractionCommand == null && InputMech.Now.InteractableObject != null)
                    {
                        InputMech.Now.InteractableObject.StartInteraction();
                    }
                }
            }

            if (currInteractionCommand != null && currInteractionCommand.Done)
            {
                if (currInteractionCommand.Selected)
                {
                    // edit the map!
                    
                }


                currInteractionCommand = null;
            }

            //var coord = World.Map.GetObjectPosition(Actor);
            //transform.localPosition = World.Map.TransformCoordinateToWorld(coord);
            //var pos = World.Map.GetPosition(Actor);

            //var worldPos = new Vector3(pos.X, 0, pos.Y) + World.Map.CellOffset;
            //Debug.DrawLine(worldPos , worldPos + Vector3.up * 1, Color.red);
        }

        public void IssueBuildCommand()
        {
            Actor.ClearCommands();
            var selectionCommand = new BoxSelectionCommand();
            //BoxSelector.x = InputMech.Now.Coord.X;
            //BoxSelector.y = InputMech.Now.Coord.Y;
            selectionCommand.Callback = () =>
            {
                var setToWalk = BoxSelector.GetCoordinates(0);
                setToWalk.ForEach(c =>
                {
                    World.Map.Handlers.Walkable.Set(World.Map.GetCell(c), true);

                });

                var toKill = BoxSelector.Viz.CollidingWith.Distinct().ToList();
                toKill.ForEach(g =>
                {
                    DestroyImmediate(g);
                });
                World.Map.AutoMap((coord, cell) => World.Map.Handlers.Walkable.Process(cell));

                var results = MapLoader.ApplyRules(World.Results.Global, World.Map, new PatternSet(), new List<Surface.Generation.Rules.SuperRule>(), setToWalk);
                World.Results.Join(results);
                World.RebuildNav();
            };
            selectionCommand.Validator = (box, rx, ry) =>
            {

                var coord = new GridXY(box.x + rx, box.y + ry);
                if (!box.World.Map.CoordinateExists(coord))
                {
                    return false;
                }
                var cell = box.World.Map.GetCell(coord);
                var walkable = box.World.CellHandlers.Walkable.Process(cell);


                return !walkable;
            };
            currInteractionCommand = selectionCommand;
            Actor.AddCommand(selectionCommand);
        }

        public void IssueMakeMoveObjectCommand()
        {
            Actor.ClearCommands();
            var moveCommand = new MoveObjectCommand();
            currInteractionCommand = moveCommand;

            Actor.AddCommand(moveCommand);
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
