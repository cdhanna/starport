using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Generation
{
    public class GenerationContext
    {

        private Dictionary<string, int> _ints = new Dictionary<string, int>();
        private Dictionary<string, string> _strs = new Dictionary<string, string>();
        private Dictionary<string, float> _floats = new Dictionary<string, float>();
        private Dictionary<string, bool> _bools = new Dictionary<string, bool>();
        private Dictionary<string, object> _objs = new Dictionary<string, object>();

        private GenerationContext _parent;

        public GenerationContext(GenerationContext parent)
        {
            _parent = parent;
        }

        //public int GetInt(string name)
        //{
        //    var def = default(int);
        //    if (_ints.TryGetValue(name, out def))
        //    {
        //        return def;
        //    }
        //    else throw new InvalidOperationException("Member does not exist in ctx, " + name);
        //}
        //public float GetFloat(string name)
        //{
        //    var def = default(float);
        //    if (_floats.TryGetValue(name, out def))
        //    {
        //        return def;
        //    }
        //    else throw new InvalidOperationException("Member does not exist in ctx, " + name);
        //}
        //public string GetString(string name)
        //{
        //    var def = default(string);
        //    if (_strs.TryGetValue(name, out def))
        //    {
        //        return def;
        //    }
        //    else throw new InvalidOperationException("Member does not exist in ctx, " + name);
        //}
        //public bool GetBool(string name)
        //{
        //    var def = default(bool);
        //    if (_bools.TryGetValue(name, out def))
        //    {
        //        return def;
        //    }
        //    else throw new InvalidOperationException("Member does not exist in ctx, " + name);
        //}
        public T Get<T>(string name)
            where T : struct
        {
            object output = null;
            if (_objs.TryGetValue(name, out output))
            {
                if (output is T)
                {
                    return (T)output;
                }
            }
            throw new InvalidOperationException("Member does not exist in ctx, " + name);

        }
        public GenerationContext Set<T>(string name, T value)
            where T : struct
        {
            if (_objs.ContainsKey(name))
                _objs[name] = value;
            else _objs.Add(name, value);
            return this;
        }


        //public GenerationContext Set(string name, int value)
        //{
        //    if (_ints.ContainsKey(name))
        //        _ints[name] = value;
        //    else _ints.Add(name, value);
        //    return this;
        //}
        //public GenerationContext Set(string name, float value)
        //{
        //    if (_floats.ContainsKey(name))
        //        _floats[name] = value;
        //    else _floats.Add(name, value);
        //    return this;
        //}
        //public GenerationContext Set(string name, string value)
        //{
        //    if (_strs.ContainsKey(name))
        //        _strs[name] = value;
        //    else _strs.Add(name, value);
        //    return this;
        //}
        //public GenerationContext Set(string name, bool value)
        //{
        //    if (_bools.ContainsKey(name))
        //        _bools[name] = value;
        //    else _bools.Add(name, value);
        //    return this;
        //}
    }
}
