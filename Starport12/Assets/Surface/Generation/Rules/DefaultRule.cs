using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    [Serializable]
    public abstract class DefaultCtxRule : GenerationRule<Ctx>
    {
    }

    //[CreateAssetMenu(fileName = "Rule", menuName = "Map/Rule/")]
    public abstract class SuperRule : MonoBehaviour
    {
        public abstract DefaultCtxRule Rule { get; }
    }
}
