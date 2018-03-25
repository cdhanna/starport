using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    public class Cell : ICell<Cell>
    {

        public const long LAYER_WALKABLE = 10;
        public const long LAYER_ROOMS = 20;
        public Dictionary<long, CellTemplate> CellData { get; set; } = new Dictionary<long, CellTemplate>()
        {
            { LAYER_WALKABLE, CellTemplates.Empty },
            { LAYER_ROOMS, CellTemplates.Empty }
        };

        //public bool Walkable { get; set; }
        //public char Code { get; set; }
        //public string Type { get; set; }

        //public GameObject DefaultFloorAsset { get; set; }
        //public GameObject DefaultWallAsset { get; set; }
        //public GameObject DefaultJoinAsset { get; set; }
        //public GameObject DefaultCornerJoinAsset { get; set; }
        //public MapTileSet ReferenceSet { get; set; }

    }
    
    //public struct CellData
    //{
    //    public byte Red, Green, Blue, Alpha;

    //    public Color Color { get { return new Color(Red / 255f, Green / 255f, Blue / 255f, Alpha / 255f); } }
    //}

    public interface ICellHandler<out TResult>
    {
        long LayerCode { get; }
        TResult Process(Cell cell);
    }

    public abstract class DefaultCellHandler<TResult> : ICellHandler<TResult>
    {
        public abstract long LayerCode { get; }

        protected abstract TResult Interp(CellTemplate data);

        public TResult Process(Cell cell)
        {
            var data = cell.CellData[LayerCode];
            return Interp(data);
        }
    }

    public class CellHandlers
    {
        //public List<DefaultCellHandler<object>> Handlers { get; set; }

        public WalkableHandler Walkable { get; set; } 
        public RoomMapTileSetHandler TileSet { get; set; }
        public CellHandlers(MapTilePalett palett)
        {
            Walkable = new WalkableHandler();
            TileSet = new RoomMapTileSetHandler(palett);
            //var walkHandler = new WalkableHandler();
            // Handlers.Add(walkHandler);
        }
    }

    public class WalkableHandler : DefaultCellHandler<bool>
    {
        public override long LayerCode { get; } = Cell.LAYER_WALKABLE;

        protected override bool Interp(CellTemplate data)
        {
            return data.Red == 255;
        }
    }

    public class RoomTypeNameHandler : DefaultCellHandler<string>
    {
        public override long LayerCode { get; } = Cell.LAYER_ROOMS;

        public Dictionary<Color, string> ColorToRoomName { get; set; }

        public RoomTypeNameHandler(Dictionary<Color, string> rooms)
        {
            ColorToRoomName = rooms;
        }

        protected override string Interp(CellTemplate data)
        {
            return ColorToRoomName[data.Color];
        }
    }

    public class RoomMapTileSetHandler : DefaultCellHandler<MapTileSet>
    {
        public override long LayerCode { get; } = Cell.LAYER_ROOMS;

        public MapTilePalett Palett { get; private set; }

        public RoomMapTileSetHandler(MapTilePalett palett)
        {
            Palett = palett;
        }

        protected override MapTileSet Interp(CellTemplate data)
        {
            var match = Palett.TileSets.FirstOrDefault(t => t.WalkColor.Equals(data.Color));
            if (match == null)
            {
                throw new Exception("No tile found for data " + data);
            }
           
            return match;
        }
    }


    //public class Room

}
