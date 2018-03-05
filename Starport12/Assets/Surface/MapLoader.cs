using Newtonsoft.Json;
using Smallgroup.Starport.Assets.Surface.CellTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class MapLoader
    {

        public MapXY LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var raw = JsonConvert.DeserializeObject<MapData>(json);

            var map = new MapXY();

            var generationFunc = raw.Basic.Binding.BuildGenerationFunc();

            for (var rowIndex = 0; rowIndex < raw.Basic.Data.Length; rowIndex++)
            {
                var row = raw.Basic.Data[rowIndex];
                for (var colIndex = 0; colIndex< row.Length; colIndex++)
                {
                    var cellCode = row[colIndex];

                    var cell = generationFunc(cellCode);
                    map.SetCell(new GridXY(rowIndex, colIndex), cell);
                }
            }

            //var existsFunc = new Func<char, bool>(code => code.Equals(raw.Basic.Binding.Exists));
            
            //for (var rowIndex = 0; rowIndex < raw.Basic.Data.Length; rowIndex ++)
            //{
            //    var row = raw.Basic.Data[rowIndex];
            //    for (var colIndex = 0; colIndex < row.Length; colIndex ++)
            //    {
            //        var code = row[colIndex];
            //        if (existsFunc(code))
            //        {
            //            map.SetCell(new GridXY(rowIndex, colIndex), new Cell());
            //        }
            //    }
            //}

            map.AutoMap();
            return map;
        }

        class MapData
        {
            [JsonProperty("basic")]
            public MapDataBasic Basic { get; set; }


            public class MapDataBasic
            {
                [JsonProperty("mappings")]
                public MapBinding Binding { get; set; }

                [JsonProperty("data")]
                public string[] Data { get; set; }
                

                public class MapBinding
                {
                    //[JsonProperty("open")]
                    //public char Open { get; set; }
                    //[JsonProperty("exist")]
                    //public char Exists { get; set; }

                    [JsonProperty("stone_walkable")]
                    public char StoneWalkable { get; set; }

                    [JsonProperty("stone_full")]
                    public char StoneFull { get; set; }

                    [JsonProperty("dirt_full")]
                    public char DirtFull { get; set; }

                    public Func<char, Cell> BuildGenerationFunc()
                    {
                        var table = new Dictionary<char, Func<Cell>>();
                        table.Add(DirtFull, () => new DirtCell(false));
                        table.Add(StoneFull, () => new StoneCell(false));
                        table.Add(StoneWalkable, () => new StoneCell(true));

                        return (code) =>
                        {
                            if (table.ContainsKey(code))
                            {
                                return table[code]();
                            } else
                            {
                                throw new Exception($"Unknown map code {code}");
                            }
                        };
                    }
                }

                //class 
            }
        }
    }
}
