using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Core
{
    public abstract class Map<TCoordinate, TCell> 
        where TCoordinate : ICoordinate<TCoordinate>, new()
        where TCell : ICell<TCell>, new()
    {

        private Dictionary<TCoordinate, TCell> _coord2Cell;
        private Dictionary<TCell, TCoordinate> _cell2Coord;

        private Dictionary<MapObjectID, TCoordinate> _objID2Coord;
        //private Dictionary<MapObjectID, IMapObject>

        //private Dictionary<Guid, Actor> _actors;

        //private Dictionary<Guid, TCellType> _objectPositions;

        public Map()
        {
            _coord2Cell = new Dictionary<TCoordinate, TCell>();
            _cell2Coord = new Dictionary<TCell, TCoordinate>();

            _objID2Coord = new Dictionary<MapObjectID, TCoordinate>();
        }

        public IEnumerable<TCoordinate> Coordinates { get { return _coord2Cell.Keys.AsEnumerable(); } }
        

        public void SetObjectPosition<TMapObject>(TCoordinate coord, TMapObject obj)
            where TMapObject : IMapObject
        {
            var existing = default(TCoordinate);
            if (_objID2Coord.TryGetValue(obj.ID, out existing))
            {
                _objID2Coord[obj.ID] = coord;
            } else
            {
                _objID2Coord.Add(obj.ID, coord);
            }
        }

        public TCoordinate GetObjectPosition(IMapObject obj)
        {
            var existing = default(TCoordinate);
            if (_objID2Coord.TryGetValue(obj.ID, out existing))
            {
                return existing;
            } else
            {
                throw new InvalidOperationException("Object does not exist in map. " + obj.ID);
            }
        }

        //public void SetPosition(Actor actor, TCoordinateType coord)
        //{
        //    var existing = default(Actor);
        //    if (_actors.TryGetValue(actor.Id, out existing) == false)
        //    {
        //        existing = actor;
        //        _actors.Add(actor.Id, actor);
        //    }

        //    var cell = _coord2Cell[coord];

        //    var previousCell = default(TCellType);
        //    if (_objectPositions.TryGetValue(existing.Id, out previousCell))
        //    {
        //        _objectPositions[existing.Id] = cell;
        //    } else
        //    {
        //        _objectPositions.Add(existing.Id, cell);
        //    }
        //}

        //public TCoordinateType GetPosition(Actor actor)
        //{
        //    var cell = _objectPositions[actor.Id];
        //    var position = _cellPositions[cell];
        //    return position;
        //}

        public void SetCell(TCoordinate coord, TCell cell)
        {
            var existing = default(TCell);

            if (_coord2Cell.TryGetValue(coord, out existing))
            {
                _coord2Cell[coord] = cell;
                _cell2Coord[cell] = coord;
            } else
            {
                _coord2Cell.Add(coord, cell);
                _cell2Coord.Add(cell, coord);
            }
            //_cellPositions.Add(cell, coord);
        }
        
        public TCell GetCell(TCoordinate coord)
        {
            var result = default(TCell);
            _coord2Cell.TryGetValue(coord, out result);
            return result;
        }
        
        public abstract TCoordinate TransformWorldToCoordinate(Vector3 position);
        public abstract Vector3 TransformCoordinateToWorld(TCoordinate coordinate);

        public bool CoordinateExists(TCoordinate coord)
        {
            return _coord2Cell.ContainsKey(coord);
        }

      
    }
}
