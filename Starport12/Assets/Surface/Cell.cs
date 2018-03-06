using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class Cell : ICell<Cell>
    {
        
        public bool Walkable { get; set; }
        public char Code { get; set; }
    }
}
