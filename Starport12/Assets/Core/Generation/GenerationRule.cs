﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Generation
{
    public abstract class GenerationRule<TContext>
        where TContext : GenerationContext
    {

        public string[] Tags { get; protected set; }
        public abstract bool[] EvaluateConditions(TContext ctx);
        public abstract List<GenerationAction> Execute(TContext ctx);

    }
    
}
