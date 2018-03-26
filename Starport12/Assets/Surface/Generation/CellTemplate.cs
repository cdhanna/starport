﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    [CreateAssetMenu(fileName = "Cell Template", menuName = "Map/Cell Template")]
    [Serializable]
    public class CellTemplate : ScriptableObject
    {

        

        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha = 255;

        public Color Color { get { return new Color(Red / 255f, Green / 255f, Blue / 255f, Alpha / 255f); } }
    }

    public static class CellTemplates
    {
        public static CellTemplate Empty { get; private set; } = new CellTemplate()
        {
            name = "Empty",
            Red = 0,
            Green = 0,
            Blue = 0,
            Alpha = 0,
        };
    }
}