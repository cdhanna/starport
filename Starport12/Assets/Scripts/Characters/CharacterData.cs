using Smallgroup.Starport.Assets.Scripts.GameResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Smallgroup.Starport.Assets.Scripts.Characters
{


    [CreateAssetMenu(fileName = "Character", menuName = "Jafar/Character")]
    public class CharacterData : ScriptableObject
    {
        public string DisplayName;
        public Sprite Icon;

        public List<GameResource> ResourceAbilities;

    }
}