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

        public string filePath;
        public List<MapTileSet> tileSets;
        //public 


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
