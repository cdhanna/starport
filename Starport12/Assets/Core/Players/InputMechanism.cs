using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Players
{
    public interface InputMechanism<out TActor> where TActor : Actor
    {
        //TActor Actor { get; set; }
    }

    [Serializable]
    public class DefaultInputMech<TActor> : InputMechanism<TActor>
        where TActor : Actor
    {
        public bool Ignore { get; set; }
        public TActor Actor { get; set; }

        public virtual void Init()
        {

        }

        public virtual void Update()
        {

        }
    }

}
