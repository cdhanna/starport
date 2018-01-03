using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Smallgroup.Starport.Assets.Core.Players
{
    class Actor
    {
        public Guid Id { get; private set; }
        public InputMechanism<Actor> InputMech { get; set; }

        public Actor()
        {
            Id = Guid.NewGuid();
        }

    }
}
