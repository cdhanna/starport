using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Core.Players
{
    public interface InputMechanism<out TActor> where TActor : Actor
    {
        //TActor Actor { get; set; }
    }

    public class DefaultInputMech<TActor> : MonoBehaviour, InputMechanism<TActor>
        where TActor : Actor
    {

        public TActor Actor { get; set; }

        //public (TActor actor)
        //{
        //    Actor = actor;
        //}
    }

}
