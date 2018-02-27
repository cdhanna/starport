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
        public Actor Speaker;
        public Actor Target;

       public OpenDialogCommand(Actor speaker, Actor target)
        {
            Speaker = speaker;
            Target = target;
        }

    }
}
