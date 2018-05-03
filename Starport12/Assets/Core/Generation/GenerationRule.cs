using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Generation
{
    [Serializable]
    public abstract class GenerationRule<TContext>
        where TContext : GenerationContext
    {

        public string Tag { get; protected set; }
        public abstract bool[] EvaluateConditions(TContext ctx);
        public abstract List<GenerationAction> Execute(TContext ctx);

        //public GenerationRule(TContext ctx)
        //{

        //}

    }
    
}
