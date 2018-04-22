using Dialog.Engine;
using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Scripts.Characters.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Smallgroup.Starport.Assets.Surface
{

    [Serializable]
    public class SimpleActor : Actor
    {
        private MapXY _map;
        private GridPather _pather;
        private DialogAnchor _dialog;
        private ActorAnchor _anchor;
        private Transform _transform;

        [Header("Coefs")]
        public Vector3 Velocity;
        public float Speed = .1f, Friction = .35f;

        [Header("Attributes")]
        //public string Name;
        //public bool Merchant, Seeker, Criminal, Collection, Trust;
        //public int Health = 100;
        //public int Respect = 50;
        public List<BagBoolElement> Flags = new List<BagBoolElement>();
        public List<BagIntElement> Ints = new List<BagIntElement>();
        public List<BagStringElement> Strs = new List<BagStringElement>();

        // TODO: REFACTOR CLASS TO ONLY HAVE DATA, I GUESS


        public void Setup(MapXY map, ActorAnchor anchor, Transform transform)
        {
            _transform = transform;
            _map = map;
            _anchor = anchor;
            _pather = new GridPather();
        }

        public void InitDialogAttributes(DialogAnchor dialog)
        {
            UnitySystemConsoleRedirector.Redirect();
            if (dialog == null) return; 

            var dEngine = dialog.dEngine;
            _dialog = dialog;

            var name = _anchor.Character.DisplayName.Replace(" ", "_");

            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".name", v => { }, () => _anchor.Character.DisplayName));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".commandable", v => {
                _anchor.Character.IsCommandable = v;
            }, () => _anchor.Character.IsCommandable));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, name, "name"));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Merchant)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Seeker)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Criminal)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Collection)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Trust)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Health)));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Respect)));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".flags", false, Flags).UpdateElements(dEngine));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".ints", 0, Ints).UpdateElements(dEngine));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".strs", "", Strs).UpdateElements(dEngine));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".position.x", x => Coordinate = new GridXY(x, Coordinate.Y), () => Coordinate.X));
            dEngine.AddAttribute(DialogAttribute.New(_anchor.Character.CodedName + ".position.y", y => Coordinate = new GridXY(Coordinate.X, y), () => Coordinate.Y));

            _anchor.Resources?.AddDialog(_anchor.Character.CodedName + ".resources", dEngine);
            //var gotoFunc = new ObjectFunctionDialogAttribute(Name + ".funcs.goto", args =>
            //{
            //    var xPosition = (int)args["x"];
            //    var yPosition = (int)args["y"];
            //    this.AddCommand(new GotoCommand() { Target = new GridXY(xPosition, yPosition) });
            //}, new Dictionary<string, object>() {
            //    { "x", -1 },
            //    { "y", -1 }
            //});
            //dEngine.AddAttribute(gotoFunc);

            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Name)));
        }

        public GridXY Coordinate
        {
            get { return _map.GetObjectPosition(this); }
            set
            {

                _map.SetObjectPosition(value, this);
            }
        }

      
        public override IEnumerable<CommandResult> ProcessCommand_Generator(ICommand command)
        {
            if (command is GotoCommand)
            {
                return HandleGoto(command as GotoCommand);
            } else if (command is OpenDialogCommand)
            {
                return HandleDialog(command as OpenDialogCommand);
            } else if (command is BoxSelectionCommand)
            {
                return HandleBoxSelection(command as BoxSelectionCommand);
            } else if (command is MoveObjectCommand)
            {
                return HandleMoveCommand(command as MoveObjectCommand);
            } else if (command is ObjectSelectionCommand)
            {
                return HandleObjectSelectCommand(command as ObjectSelectionCommand);
            }

            return null;
            //yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleObjectSelectCommand(ObjectSelectionCommand command)
        {
            yield return CommandResult.WORKING; // wait a tick for another click.
            var input = _anchor.InputMech;

            var selected = default(InteractionSupport);
            var originalPosition = new Vector3();
            while (selected == null)
            {
                if (input.NewSecondary)
                {
                    command.Done = true;
                    yield return CommandResult.COMPLETE;
                }

                if (input.Now.InteractableObject != null && input.NewPrimary)
                {
                    selected = input.Now.InteractableObject;
                    originalPosition = selected.transform.position;
                }

                yield return CommandResult.WORKING;

            }

            command.Done = true;
            if (selected != default(InteractionSupport)){
                command.Selected = true;
                command.SelectedObject = selected;
            }
            command.Callback();
            yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleMoveCommand(MoveObjectCommand command)
        {
            yield return CommandResult.WORKING; // wait a tick for another click.
            var input = _anchor.InputMech;

            var selected = default(InteractionSupport);
            var originalPosition = new Vector3();
            while (selected == null)
            {
                if (input.NewSecondary)
                {
                    command.Done = true;
                    yield return CommandResult.COMPLETE;
                }

                if (input.Now.InteractableObject != null && input.NewPrimary)
                {
                    selected = input.Now.InteractableObject;
                    originalPosition = selected.transform.position;
                }

                yield return CommandResult.WORKING; 

            }

            yield return CommandResult.WORKING; // wait a tick for another click.

            while (!input.NewPrimary)
            {
                if (input.NewSecondary)
                {
                    command.Done = true;
                    selected.transform.position = originalPosition;
                    yield return CommandResult.COMPLETE;
                }

                selected.transform.position = new Vector3(input.Now.Point.x, originalPosition.y, input.Now.Point.y);
                yield return CommandResult.WORKING; 

            }

            command.Done = true;
            _anchor.World.RebuildNav();
            yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleBoxSelection(BoxSelectionCommand command)
        {
            //yield return CommandResult.WORKING;

            yield return CommandResult.WORKING; // wait a tick for another click.

            var input = _anchor.InputMech;
            var valid = false;

            //var startCoord
            _anchor.BoxSelector.Viz.UseValidator(command.Validator);

            _anchor.BoxSelector.gameObject.SetActive(true);

            while (!input.NewPrimary)
            {
                if (input.NewSecondary)
                {
                    command.Done = true;
                    _anchor.BoxSelector.gameObject.SetActive(false);

                    yield return CommandResult.COMPLETE;
                }
                var coord = input.Now.Coord;

                _anchor.BoxSelector.x = coord.X;
                _anchor.BoxSelector.y = coord.Y;

                _anchor.BoxSelector.width = 0;
                _anchor.BoxSelector.height = 0;

                yield return CommandResult.WORKING;
            }

            var startCoord = input.Now.Coord;
            yield return CommandResult.WORKING; // wait a tick for another click.

            while (!input.NewPrimary)
            {

                if (input.NewSecondary)
                {
                    command.Done = true;
                    _anchor.BoxSelector.gameObject.SetActive(false);

                    yield return CommandResult.COMPLETE;
                }

                var coord = input.Now.Coord;

                var minX = Math.Min(startCoord.X, coord.X);
                var maxX = Math.Max(startCoord.X, coord.X);
                var minY = Math.Min(startCoord.Y, coord.Y);
                var maxY = Math.Max(startCoord.Y, coord.Y);

                _anchor.BoxSelector.x = minX;
                _anchor.BoxSelector.y = minY;

                _anchor.BoxSelector.width = maxX - minX;
                _anchor.BoxSelector.height = maxY - minY;


                yield return CommandResult.WORKING;

            }

            valid = _anchor.BoxSelector.GetRelativeCoordinates().Select(c => command.Validator(_anchor.BoxSelector, c.X, c.Y)).All(t => t);

            _anchor.BoxSelector.gameObject.SetActive(false);

            if (valid)
            {
                // use selection!
                command.Selected = true;
                command.Callback();
                _anchor.World.RebuildNav();

            }

            command.Done = true;


            yield return CommandResult.COMPLETE;

        }

        private IEnumerable<CommandResult> HandleDialog(OpenDialogCommand command)
        {
            _dialog.OpenDialog(_anchor, command.Target);
            while (_dialog.IsDialogOpen)
            {
                yield return CommandResult.WORKING;
            }

            yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleGoto(GotoCommand command)
        {
            
            var agent = _anchor.GetComponent<NavMeshAgent>();
            agent.SetDestination(command.Actual);

            while (!agent.isStopped)
            {
                yield return CommandResult.WORKING;
            }

            yield return CommandResult.COMPLETE;
        }

    }
}
