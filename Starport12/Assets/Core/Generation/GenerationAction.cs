using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Generation
{
    public abstract class GenerationAction
    {
        public abstract void Invoke(GenerationContext ctx);
    }
}
