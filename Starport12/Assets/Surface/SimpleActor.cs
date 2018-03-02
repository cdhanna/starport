using Dialog.Engine;
using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{

    [Serializable]
    public class SimpleActor : Actor
    {
        private MapXY _map;
        private GridPather _pather;
        private DialogAnchor _dialog;

        private Transform _transform;

        [Header("Coefs")]
        public Vector3 Velocity;
        public float Speed = .1f, Friction = .35f;

        [Header("Attributes")]
        public string Name;
        public bool Merchant, Seeker, Criminal, Collection, Trust;
        public int Health = 100;
        public int Respect = 50;
        public List<BagBoolElement> Flags = new List<BagBoolElement>();
        public List<BagIntElement> Ints = new List<BagIntElement>();
        public List<BagStringElement> Strs = new List<BagStringElement>();

        // TODO: REFACTOR CLASS TO ONLY HAVE DATA, I GUESS


        public void Setup(MapXY map, Transform transform)
        {
            _transform = transform;
            _map = map;
            _pather = new GridPather();
        }

        public void InitDialogAttributes(DialogAnchor dialog)
        {
            UnitySystemConsoleRedirector.Redirect();

            var dEngine = dialog.dEngine;
            _dialog = dialog;
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Name)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Merchant)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Seeker)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Criminal)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Collection)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Trust)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Health)));
            dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Respect)));
            dEngine.AddAttribute(DialogAttribute.New(Name + ".flags", false, Flags).UpdateElements(dEngine));
            dEngine.AddAttribute(DialogAttribute.New(Name + ".ints", 0, Ints).UpdateElements(dEngine));
            dEngine.AddAttribute(DialogAttribute.New(Name + ".strs", "", Strs).UpdateElements(dEngine));
            //dEngine.AddAttribute(new ObjectDialogAttribute(this, Name, nameof(Name)));
        }

        public GridXY Coordinate { get { return _map.GetObjectPosition(this); } }

        public void MoveLeft()
        {
            var current = _map.GetObjectPosition(this);
            current.X -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveRight()
        {
            var current = _map.GetObjectPosition(this);
            current.X += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveUp()
        {
            var current = _map.GetObjectPosition(this);
            current.Y += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveDown()
        {
            var current = _map.GetObjectPosition(this);
            current.Y -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
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
            }

            return null;
            //yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleDialog(OpenDialogCommand command)
        {
            _dialog.OpenDialog();
            while (_dialog.IsDialogOpen)
            {
                yield return CommandResult.WORKING;
            }

            yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleGoto(GotoCommand command)
        {
            var path = _pather.FindPath(_map, _map.GetObjectPosition(this), command.Target);


            for (var i = 1; i < path.Count; i++)
            {
                
                //var startPos = _transform.position;

                var targetPos = _map.TransformCoordinateToWorld(path[i]);

                var mag = (targetPos - _transform.position).magnitude;
                while ( mag > .2f)
                {

                    var pullForce = (targetPos - _transform.position).normalized * Speed;
                    var frictionForce = (Velocity * -Friction);
                    var acceleration = (pullForce + frictionForce) / 1.0f;

                    Velocity += acceleration;
                    _transform.position += Velocity;
                    mag = (targetPos - _transform.position).magnitude;
                    yield return CommandResult.WORKING;

                }
                _map.SetObjectPosition(path[i], this);


                //var placedAt = DateTime.Now;
                //var procedeAt = placedAt.AddMilliseconds(250);

                //var startPos = _transform.position;
                //var endPos = startPos;
                //if (i + 1 < path.Count)
                //{
                //    endPos = _map.TransformCoordinateToWorld(path[i + 1]);
                //}

                //while (procedeAt > DateTime.Now)
                //{
                //    var ratio = (procedeAt - DateTime.Now).Milliseconds / 250f;



                //    //if (i + 1 < path.Count)
                //    //{
                //    //    var next = path[i + 1];
                //    //    var startPos = _map.TransformCoordinateToWorld(node);
                //    //    var endPos = _map.TransformCoordinateToWorld(next);

                //    var pos = startPos + ratio * (endPos - startPos);
                //    //_transform.position = pos;
                //    //}

                //    yield return CommandResult.WORKING;
                //}
                //var node = path[i];
                //_map.SetObjectPosition(node, this);
            }


            Debug.Log("DONE");
            yield return CommandResult.COMPLETE;

        }

    }
}
