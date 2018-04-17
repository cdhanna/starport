using Smallgroup.Starport.Assets.Core.Players;
using Smallgroup.Starport.Assets.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Characters.Commands
{



    class BoxSelectionCommand : InteractionBasedCommand
    {


        public IsCellValidFunc Validator { get; set; }
        public BoxSelectionCommand()
        {

            if (Validator == null)
            {
                Validator = (selection, x, y) => true;
            }
        }

        
        
    }
}
