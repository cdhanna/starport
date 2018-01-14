using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core
{
    public struct MapObjectID
    {
        public static MapObjectID New()
        {
            return new MapObjectID(Guid.NewGuid());
        }

        public Guid ID;

        public MapObjectID(Guid id)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is MapObjectID)
            {
                var other = (MapObjectID)obj;
                return other.ID.Equals(ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }

    public interface IMapObject
    {
        MapObjectID ID { get; }
    }
}
