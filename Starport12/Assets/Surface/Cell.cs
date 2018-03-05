using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class Cell : ICell<Cell>
    {
        public string TypeName { get; protected set; }
        //public char MapCode { get; protected set; }
        public bool Walkable { get; set; }

        public string FillPrefabName { get; protected set; }

    }
}
