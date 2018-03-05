using Dialog.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Dialogue
{
    [Serializable]
    public class BagVec2Element : BagElement<Vector2> { }

    public class BagVec2Handler : BagAttributesRuleAddedHandler<Vector2, BagVec2Element>
    {

    }
}
