using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public static class RuleConstants
    {
        public const string CELL_X = "cell_x";
        public const string CELL_Y = "cell_y";

    }

    public class Ctx : GenerationContext
    {

        public Ctx(GenerationContext parent) : base(parent) { }

        public int CellX()
        {
            return Get<int>(RuleConstants.CELL_X);
        }
        public int CellY()
        {
            return Get<int>(RuleConstants.CELL_Y);
        }
    }
}
