using Newtonsoft.Json;
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
            var existsFunc = new Func<char, bool>(code => code.Equals(raw.Basic.Binding.Exists));
            
            for (var rowIndex = 0; rowIndex < raw.Basic.Data.Length; rowIndex ++)
            {
                var row = raw.Basic.Data[rowIndex];
                for (var colIndex = 0; colIndex < row.Length; colIndex ++)
                {
                    var code = row[colIndex];

                    var walkable = existsFunc(code);
                    //if (existsFunc(code))
                    //{
                    //}
                    // TODO make cell types scriptable objects that we can load in
                    map.SetCell(new GridXY(colIndex, -rowIndex + raw.Basic.Data.Length), new Cell()
                    {
                        Walkable = walkable,
                        Code = code
                    });
                }
            }

            map.AutoMap( (coord, cell) => cell.Walkable);
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
                    [JsonProperty("open")]
                    public char Open { get; set; }
                    [JsonProperty("exist")]
                    public char Exists { get; set; }
                }

                //class 
            }
        }
    }
}
