using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Surface
{
    public class OpenDialogCommand : ICommand
    {
        public SimpleActor Target;

       public OpenDialogCommand(SimpleActor target)
        {
            Target = target;
        }

    }
}
