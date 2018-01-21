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

        public GenerationContext()
        {
            _parent = null;
        }
        public void SetParent(GenerationContext parent)
        {
            _parent = parent;
        }


        public virtual void SetContextInfo<T>(T info)
        {
            // do nothing...
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

            if (_parent != null)
            {
                return _parent.Get<T>(name);
            }

            throw new InvalidOperationException("Member does not exist in ctx, " + name);

        }

        public string Get(string name)
        {
            object output = null;
            if (_objs.TryGetValue(name, out output))
            {
                if (output is string)
                {
                    return (string)output;
                }
            }
            if (_parent != null)
            {
                return _parent.Get(name);
            }
            throw new InvalidOperationException("Member does not exist in ctx, " + name);

        }
        public GenerationContext Set(string name, string value)
        {
            if (_objs.ContainsKey(name))
                _objs[name] = value;
            else _objs.Add(name, value);
            return this;
        }

        public GenerationContext Set<T>(string name, T value)
            where T : struct
        {
            if (_objs.ContainsKey(name))
                _objs[name] = value;
            else _objs.Add(name, value);
            return this;
        }

        public GenerationContext SetSubContext<TCoordinate>(TCoordinate coord, GenerationContext ctx)
            where TCoordinate : ICoordinate<TCoordinate>, new()
        {
            var name = "coordCtx" + coord.GetHashCode();
            if (_objs.ContainsKey(name))
                _objs[name] = ctx;
            else _objs.Add(name, ctx);
            return this;
        }

        public TContext GetSubContext<TCoordinate, TContext>(TCoordinate coord)
            where TCoordinate : ICoordinate<TCoordinate>, new()
            where TContext : GenerationContext
        {
            object output = null;
            var name = "coordCtx" + coord.GetHashCode();

            if (_objs.TryGetValue(name, out output))
            {
                if (output is TContext)
                {
                    return (TContext)output;
                }
            }
            if (_parent != null)
            {
                return _parent.GetSubContext<TCoordinate, TContext>(coord);
            }
            throw new InvalidOperationException("Member does not exist in ctx, " + name);
        }


        public bool Exists(string name)
        {
            if (_objs.ContainsKey(name))
            {
                return true;
            } else if (_parent != null)
            {
                return _parent.Exists(name);
            } else
            {
                return false;
            }
        }

        public void Ensure<T>(string name, T defaultValue)
        {
            if (!Exists(name))
            {
                _objs.Add(name, defaultValue);
            }
        }

        //public bool EnsureExists<T>(string name, T defaultValue)
        //{
        //    if (_objs.ContainsKey(name))
        //    {
        //        return true;
        //    } else if (_parent != null)
        //    {
        //        return _parent.EnsureExists
        //    }
        //}


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
