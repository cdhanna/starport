using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.GameResources
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Jafar/Resource")]
    public class GameResourceType : ScriptableObject
    {

        public string Description;
        public string Name { get { return name; } }
        public Color Color;
        public Sprite Icon;

        public Color ColorNoTransparent { get { return new Color(Color.r, Color.g, Color.b); } }
    }
}
