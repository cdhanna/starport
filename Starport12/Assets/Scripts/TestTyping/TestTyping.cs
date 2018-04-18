using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Binding
{
    public string name;
    public int resourceType;
}

[Serializable]
public class SubBinding : Binding
{

    public int scaleFunction;
}

public abstract class TestTyping : ScriptableObject {

    protected List<TestArgBase> Behavours;

    public abstract List<Binding> GetBindings();

}


[CreateAssetMenu(fileName = "TestObject2", menuName = "TestObject2")]
public class TestTypingExample : TestTyping
{
    public TestArgMft Pattern;
    public TestArgNumber Cost;

    public List<SubBinding> subBindings;

    public override List<Binding> GetBindings()
    {
        return subBindings.Cast<Binding>().ToList();
    }
}
