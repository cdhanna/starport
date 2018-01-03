using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    class SimpleKeyboardInput : MonoBehaviour, InputMechanism<SimpleActor>
    {
        public SimpleActor Actor { get; set; }
        public SimpleKeyboardInput()
        {
        }


        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Actor.MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Actor.MoveRight();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Actor.MoveUp();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Actor.MoveDown();
            }

        }
    }
}
