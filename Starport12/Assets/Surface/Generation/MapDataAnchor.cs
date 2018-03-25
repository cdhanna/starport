using MapFileCodec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class MapDataAnchor : MonoBehaviour
    {
        //[SerializeField]
        //public MapFile MapFile;

        [SerializeField]
        public int ByteSize;

        [HideInInspector]
        public byte[] Raw;
    }
}
