using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    public class GotoCommand : ICommand
    {

        public GridXY Target { get; set; }
        public Vector3 Actual { get; set; }
        public GotoCommand()
        {
            Target = new GridXY();
        } 

        public GotoCommand(GridXY target, Vector3 actual)
        {
            Actual = actual;
            Target = target;
        }

    }
}
