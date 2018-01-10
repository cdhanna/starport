using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Players
{
    public interface InputMechanism<out TActor> where TActor : Actor
    {
    }
}
