using Smallgroup.Starport.Assets.Surface;
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

    public struct InputMoment
    {
        public Vector2 Point;
        public GridXY Coord;
        public bool Primary;
        public bool Secondary;
        public InteractionSupport InteractableObject;
    }

    [Serializable]
    public class DefaultInputMech<TActor> : InputMechanism<TActor>
        where TActor : Actor
    {
        public bool Ignore { get; set; }
        public TActor Actor { get; set; }
        
        public InputMoment Now { get; set; }
        public InputMoment Previous { get; set; }


        public bool NewPrimary
        {
            get { return Now.Primary && !Previous.Primary; }
        }
        public bool NewSecondary
        {
            get { return Now.Secondary && !Previous.Secondary; }
        }
        public virtual void Init()
        {

        }

        public void Update()
        {
            Previous = Now;
            Now = GetMoment();
        }


        protected virtual InputMoment GetMoment()
        {
            return new InputMoment();
        }

    }

}
