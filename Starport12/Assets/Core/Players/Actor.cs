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
        public ICommand ActiveCommand { get; set; }
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
            ActiveCommand = null;
            _commandQueue.Clear();
            _commandIterator = null;
        }

        public void Update()
        {
            if (_commandIterator == null && _commandQueue.Count > 0)
            {
                ActiveCommand = _commandQueue.Dequeue();
                
                var results = ProcessCommand_Generator(ActiveCommand);
                _commandIterator = results.GetEnumerator();
            }

            if (_commandIterator != null)
            {
                
                if (_commandIterator.MoveNext())
                {
                    var result = _commandIterator.Current;
                    if (result == CommandResult.FAILURE)
                    {
                        ActiveCommand = null;

                        throw new InvalidOperationException("Command ended in failure");
                    }
                    if (result == CommandResult.COMPLETE)
                    {
                        _commandIterator = null; // prepares for next.
                        ActiveCommand = null;
                    }
                    if (result == CommandResult.WORKING)
                    {
                        // do nothing, because command is working....
                    }
                } else
                {
                    // out of sequence, implies "error"
                    _commandIterator = null;
                    ActiveCommand = null;

                }
            }
        }

        public abstract IEnumerable<CommandResult> ProcessCommand_Generator(ICommand command);

    }
}
