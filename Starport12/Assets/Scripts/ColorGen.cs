using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    public static class ColorGen
    {
        public static Color FromRGB(byte red, byte green, byte blue, byte alpha)
        {
            return new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
        }
    }
}
