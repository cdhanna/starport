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
                    if (existsFunc(code))
                    {
                        map.SetCell(new GridXY(rowIndex, colIndex), new Cell());
                    }
                }
            }

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
