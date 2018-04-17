using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Characters.Commands
{
    public class InteractionBasedCommand : ICommand
    {
        public bool Done;
        public bool Selected;
        public Action Callback = () => { };
    }
}
