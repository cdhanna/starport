using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Scripts;
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
        public const long LAYER_ZONES = 30;
        public const long LAYER_REPLACE = 40;
        public Dictionary<long, CellTemplate> CellData { get; set; } = new Dictionary<long, CellTemplate>()
        {
            { LAYER_WALKABLE, CellTemplates.Empty },
            { LAYER_ROOMS, CellTemplates.Empty },
            { LAYER_ZONES, CellTemplates.Empty },
            { LAYER_REPLACE, CellTemplates.Empty }
        };

        //public Dictionary<>
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

        public abstract TResult Interp(CellTemplate data);
        public virtual void InterpSet(CellTemplate data, TResult arg)
        {
            throw new NotImplementedException($"Cant set cell because no set behavour exists. Layer={LayerCode} HandlerType={this.GetType()} Cell={data}");
            
        }
        public TResult Process(Cell cell)
        {
            if (cell.CellData.ContainsKey(LayerCode))
            {
                var data = cell.CellData[LayerCode];
                return Interp(data);
            } else
            {
                throw new Exception($"Cant process cell because layer did not exist. Layer={LayerCode} HandlerType={this.GetType()} Cell={cell}");
            }
        }

        public void Set(Cell cell, TResult value)
        {
            if (cell.CellData.ContainsKey(LayerCode))
            {
                var data = cell.CellData[LayerCode];
                InterpSet(data, value);
            } else
            {
                throw new Exception($"Cant set cell because layer did not exist. Layer={LayerCode} HandlerType={this.GetType()} Cell={cell}");
            }
        }
    }

    public class CellHandlers
    {
        //public List<DefaultCellHandler<object>> Handlers { get; set; }

        public WalkableHandler Walkable { get; set; } 
        public RoomMapTileSetHandler TileSet { get; set; }
        public MapZoneHandler Zones { get; set; }
        public ReplacementHandler Replacable { get; set; }
        public CellHandlers(MapTilePalett palett)
        {
            Walkable = new WalkableHandler();
            TileSet = new RoomMapTileSetHandler(palett.TileSets.First(), palett);
            Zones = new MapZoneHandler();
            Replacable = new ReplacementHandler();
            //var walkHandler = new WalkableHandler();
            // Handlers.Add(walkHandler);
        }
    }

    public class ReplacementHandler : DefaultCellHandler<bool>
    {
        public override long LayerCode { get; } = Cell.LAYER_REPLACE;

        public override bool Interp(CellTemplate data)
        {
           
            return !data.HadData || data.Red > 0; // you cant replace it if it doesnt have any red
        }
    }

    public class WalkableHandler : DefaultCellHandler<bool>
    {
        public override long LayerCode { get; } = Cell.LAYER_WALKABLE;

        public override bool Interp(CellTemplate data)
        {
            return data.Red == 255;
        }

        public override void InterpSet(CellTemplate data, bool arg)
        {
            data.Red = (arg == true ? (byte)255 : (byte)0);
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

        public override string Interp(CellTemplate data)
        {
            return ColorToRoomName[data.Color];
        }
    }

    public class MapZoneHandler : DefaultCellHandler<MapZone>
    {
        public override long LayerCode { get; } = Cell.LAYER_ZONES;

        public List<MapZone> Zones { get; set; }

        public override MapZone Interp(CellTemplate data)
        {
            return Zones.FirstOrDefault(z => z.ColorCode.Equals(data.Color));
        }
    }

    public class RoomMapTileSetHandler : DefaultCellHandler<MapTileSet>
    {
        public override long LayerCode { get; } = Cell.LAYER_ROOMS;

        public MapTilePalett Palett { get; private set; }
        public MapTileSet EmptyTileSet { get; private set; }

        public RoomMapTileSetHandler(MapTileSet nullTileset, MapTilePalett palett)
        {
            Palett = palett;
            EmptyTileSet = nullTileset;
        }

        public override MapTileSet Interp(CellTemplate data)
        {
            var match = Palett.TileSets.FirstOrDefault(t => t.WalkColor.Equals(data.Color));
            if (match == null)
            {
                return EmptyTileSet;
                //throw new Exception("No tile found for data " + data);
            }

            return match;
        }
    }


    //public class Room

}
