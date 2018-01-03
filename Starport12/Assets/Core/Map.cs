using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core
{
    class Map<TCoordinateType> 
        where TCoordinateType : ICoordinate<TCoordinateType>, new()
    {

        private Dictionary<TCoordinateType, Cell> _world;

        private Dictionary<Guid, Actor> _actors;

        private Dictionary<Guid, Cell> _objectPositions;
        private Dictionary<Cell, TCoordinateType> _cellPositions;

        public Map()
        {
            _world = new Dictionary<TCoordinateType, Cell>();
            _actors = new Dictionary<Guid, Actor>();
            _objectPositions = new Dictionary<Guid, Cell>();
            _cellPositions = new Dictionary<Cell, TCoordinateType>();
        }

        public IEnumerable<TCoordinateType> Coordinates { get { return _world.Keys.AsEnumerable(); } }
        
        public void SetPosition(Actor actor, TCoordinateType coord)
        {
            var existing = default(Actor);
            if (_actors.TryGetValue(actor.Id, out existing) == false)
            {
                existing = actor;
                _actors.Add(actor.Id, actor);
            }

            var cell = _world[coord];

            var previousCell = default(Cell);
            if (_objectPositions.TryGetValue(existing.Id, out previousCell))
            {
                _objectPositions[existing.Id] = cell;
            } else
            {
                _objectPositions.Add(existing.Id, cell);
            }
        }

        public TCoordinateType GetPosition(Actor actor)
        {
            var cell = _objectPositions[actor.Id];
            var position = _cellPositions[cell];
            return position;
        }

        public void Set(TCoordinateType coord, Cell cell)
        {
            var existing = default(Cell);
            if (_world.TryGetValue(coord, out existing))
            {
                _world[coord] = cell;
            } else
            {
                _world.Add(coord, cell);
            }
            _cellPositions.Add(cell, coord);
        }
        
        public bool CoordinateExists(TCoordinateType coord)
        {
            return _world.ContainsKey(coord);
        }
        

    }
}
