using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class OpenDialogCommand : ICommand
    {
        public ActorAnchor Target;

       public OpenDialogCommand(ActorAnchor target)
        {
            Target = target;
        }

    }
}
