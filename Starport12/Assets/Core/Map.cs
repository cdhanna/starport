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

        private Dictionary<TCoordinate, List<TCoordinate>> _coord2TraversableCoords;

        public Map()
        {
            _coord2Cell = new Dictionary<TCoordinate, TCell>();
            _cell2Coord = new Dictionary<TCell, TCoordinate>();

            _objID2Coord = new Dictionary<MapObjectID, TCoordinate>();
            _coord2TraversableCoords = new Dictionary<TCoordinate, List<TCoordinate>>();
        }

        public TCoordinate[] Coordinates { get { return _coord2Cell.Keys.ToArray(); } }
        

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
        }
        
        public TCell GetCell(TCoordinate coord)
        {
            var result = default(TCell);
            _coord2Cell.TryGetValue(coord, out result);
            return result;
        }

        public List<TCoordinate> GetTraversable(TCoordinate from)
        {
            var coords = default(List<TCoordinate>);
            if (_coord2TraversableCoords.TryGetValue(from, out coords))
            {
                return coords;
            }
            else return new List<TCoordinate>();
        }

        public void SetTraversable(TCoordinate from, List<TCoordinate> traversable)
        {
            if (_coord2TraversableCoords.ContainsKey(from))
            {
                _coord2TraversableCoords[from] = traversable;
            } else
            {
                _coord2TraversableCoords.Add(from, traversable);
            }
        }

        public void AutoMap()
        {
            // take all coords, and figure out their traversable paths. This assumes no walls, just "does the neighbor coord exist"

            foreach (var coord in _coord2Cell.Keys.ToList())
            {
                var neighbors = coord.GetNeighbors();
                var traversable = neighbors.Where(n => CoordinateExists(n)).ToList();
                SetTraversable(coord, traversable);
            }
        }
        
        public abstract TCoordinate TransformWorldToCoordinate(Vector3 position);
        public abstract Vector3 TransformCoordinateToWorld(TCoordinate coordinate);

        public bool CoordinateExists(TCoordinate coord)
        {
            return _coord2Cell.ContainsKey(coord);
        }

      
    }
}
