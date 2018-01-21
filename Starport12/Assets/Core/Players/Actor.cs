using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Smallgroup.Starport.Assets.Core.Players
{
    [Serializable]
    public abstract class Actor : IMapObject
    {
        public InputMechanism<Actor> InputMech { get; set; }

        private MapObjectID _id = MapObjectID.New();
        public MapObjectID ID { get { return _id; } }

        protected Queue<ICommand> _commandQueue;
        protected ICommand _activeCommand;
        protected IEnumerable<CommandResult> _activeCommandResults;
        protected IEnumerator<CommandResult> _commandIterator;

        public Actor()
        {
            _commandQueue = new Queue<ICommand>();
        }

        public virtual void AddCommand(ICommand command)
        {
            _commandQueue.Enqueue(command);
        }

        public virtual void ClearCommands()
        {
            // cancel current command ?
            _activeCommand = null;
            _commandQueue.Clear();
            _commandIterator = null;
        }

        public void Update()
        {
            if (_commandIterator == null && _commandQueue.Count > 0)
            {
                _activeCommand = _commandQueue.Dequeue();
                
                var results = ProcessCommand_Generator(_activeCommand);
                _commandIterator = results.GetEnumerator();
            }

            if (_commandIterator != null)
            {
                
                if (_commandIterator.MoveNext())
                {
                    var result = _commandIterator.Current;
                    if (result == CommandResult.FAILURE)
                    {
                        throw new InvalidOperationException("Command ended in failure");
                    }
                    if (result == CommandResult.COMPLETE)
                    {
                        _commandIterator = null; // prepares for next.
                    }
                    if (result == CommandResult.WORKING)
                    {
                        // do nothing, because command is working....
                    }
                } else
                {
                    // out of sequence, implies "error"
                    _commandIterator = null;
                }
            }
        }

        public abstract IEnumerable<CommandResult> ProcessCommand_Generator(ICommand command);

    }
}
