using Newtonsoft.Json;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class MapLoader : MonoBehaviour
    {
        public string mftFilePath;


        public string filePath;
        public List<MapTileSet> tileSets;
        //public 

        public MapXY LoadFromMFT()
        {
            var bytes = File.ReadAllBytes(mftFilePath);
            var mapFile = MapFileCodec.Converter.FromBytes(bytes);
            var map = new MapXY();

            for (var y = 0; y < mapFile.Height; y++)
            {
                for (var x = 0; x < mapFile.Width; x++)
                {
                    var roomData = mapFile.GetData("rooms", x, y);
                    var walkableData = mapFile.GetData("walkable", x, y);
                    var cell = GenerateCell(roomData.ChannelR, roomData.ChannelG, roomData.ChannelB, roomData.ChannelA);
                    cell.Walkable = walkableData.ChannelB == 255;
                    if (cell != null)
                    {
                        map.SetCell(new GridXY(x, -y + mapFile.Height), cell);

                    }
                }
            }

            map.AutoMap( (coord, cell) => cell.Walkable);
            return map;
        }

        public MapXY LoadFromFile()
        {
            var json = File.ReadAllText(filePath);
            var raw = JsonConvert.DeserializeObject<MapData>(json);

            var map = new MapXY();
           
            for (var rowIndex = 0; rowIndex < raw.Basic.Data.Length; rowIndex ++)
            {
                var row = raw.Basic.Data[rowIndex];
                for (var colIndex = 0; colIndex < row.Length; colIndex ++)
                {
                    var code = row[colIndex];
                    var cell = GenerateCell(code);
                    //var cell = gen(code);
                    map.SetCell(new GridXY(colIndex, -rowIndex + raw.Basic.Data.Length), cell);
                }
            }

            map.AutoMap( (coord, cell) => cell.Walkable);
            return map;
        }

        public Cell GenerateCell(byte red, byte green, byte blue, byte alpha)
        {
            var r = red / 255f;
            var g = green / 255f;
            var b = blue / 255f;
            var a = alpha / 255f;
            var set = tileSets.FirstOrDefault(t => t.WalkColor.r == r && t.WalkColor.g == g && t.WalkColor.b == b);
            if (set != null)
            {

                return new Cell()
                {
                    Walkable = true,
                    Code = set.WalkableCode,
                    DefaultCornerJoinAsset = set.CornerJoinPrefab,
                    DefaultJoinAsset = set.JoinPrefab,
                    DefaultWallAsset = set.WallPrefab,
                    Type = set.name,
                    ReferenceSet = set,
                    DefaultFloorAsset = set.FloorPrefab,
                };
            }
            else return null;
        }

        public Cell GenerateCell(char code)
        {
            var fill = tileSets.FirstOrDefault(t => t.FillCode == code);
            var walk = tileSets.FirstOrDefault(t => t.WalkableCode == code);

            if (fill != null)
            {
                return new Cell()
                {
                    Walkable = false,
                    Code = code,
                    DefaultFloorAsset = fill.FloorPrefab,
                    DefaultWallAsset = fill.WallPrefab,
                    DefaultJoinAsset = fill.JoinPrefab,
                    DefaultCornerJoinAsset = fill.CornerJoinPrefab

                };
            }

            if (walk != null)
            {
                return new Cell()
                {
                    Walkable = true,
                    Code = code,
                    DefaultFloorAsset = walk.FloorPrefab,
                    DefaultWallAsset = walk.WallPrefab,
                    DefaultJoinAsset = walk.JoinPrefab,
                    DefaultCornerJoinAsset = walk.CornerJoinPrefab
                };
            }

            throw new Exception("invalid map code exception. What is the code, '" + code + "' ?");
        }


        class MapData
        {
            [JsonProperty("basic")]
            public MapDataBasic Basic { get; set; }

            public class MapDataBasic
            {
                
                [JsonProperty("data")]
                public string[] Data { get; set; }

               
                //class 
            }
        }
    }
}
