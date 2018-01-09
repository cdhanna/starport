using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Core
{
    abstract class Map<TCoordinateType, TCellType> 
        where TCoordinateType : ICoordinate<TCoordinateType>, new()
        where TCellType : ICell<TCellType>, new()
    {

        private Dictionary<TCoordinateType, TCellType> _world;
        private Dictionary<int, TCoordinateType> _similarHashCodeToActualCoordinate;

        private Dictionary<Guid, Actor> _actors;

        private Dictionary<Guid, TCellType> _objectPositions;
        private Dictionary<TCellType, TCoordinateType> _cellPositions;

        public Map()
        {
            _similarHashCodeToActualCoordinate = new Dictionary<int, TCoordinateType>();
            _world = new Dictionary<TCoordinateType, TCellType>();
            _actors = new Dictionary<Guid, Actor>();
            _objectPositions = new Dictionary<Guid, TCellType>();
            _cellPositions = new Dictionary<TCellType, TCoordinateType>();
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

            var previousCell = default(TCellType);
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

        public void Set(TCoordinateType coord, TCellType cell)
        {
            var existing = default(TCellType);
            if (_world.TryGetValue(coord, out existing))
            {
                _world[coord] = cell;
            } else
            {
                _world.Add(coord, cell);
            }
            _cellPositions.Add(cell, coord);
        }
        
        public TCellType Get(TCoordinateType coord)
        {
            var result = default(TCellType);
            _world.TryGetValue(coord, out result);
            return result;
        }

        public TCoordinateType GetRightCoord(TCoordinateType coord)
        {
            var result = default(TCoordinateType);
            var hash = coord.GetSimilarHashCode();
            _similarHashCodeToActualCoordinate.TryGetValue(hash, out result);
            return result;
        }

        public abstract TCoordinateType TransformWorldToCoordinate(Vector3 position);

        public bool CoordinateExists(TCoordinateType coord)
        {
            return _world.ContainsKey(coord);
        }

      
    }
}
