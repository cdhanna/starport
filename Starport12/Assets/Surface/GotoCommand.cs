using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class GotoCommand : ICommand
    {

        public GridXY Target { get; set; }

        public GotoCommand()
        {
            Target = new GridXY();
        } 

        public GotoCommand(GridXY target)
        {
            Target = target;
        }

    }
}
