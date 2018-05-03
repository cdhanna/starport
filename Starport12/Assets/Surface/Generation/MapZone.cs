using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    [CreateAssetMenu(fileName = "MapZone", menuName = "Map/Map Zone")]
    public class MapZone : ScriptableObject
    {
        public Color ColorCode;
    }

    public class SpawnToZone : MonoBehaviour
    {
        public MapZone SpawnZone;



        private void Awake()
        {
            
        }
    }
}
