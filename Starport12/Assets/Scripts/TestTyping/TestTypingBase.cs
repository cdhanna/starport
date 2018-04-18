using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TestArgBase
{
    public string name;
}

[Serializable]
public class TestArgNumber : TestArgBase
{
    public bool useMin, useMax;
    public int min, max;
}

[Serializable]
public class TestArgMft : TestArgBase
{
    public bool scale;
    public MapDataAnchor Mft;
}
