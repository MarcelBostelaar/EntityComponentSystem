using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.IDs
{
    public struct EntityID : IComparable<EntityID>, IEqualityComparer<EntityID>
    {
        private readonly ulong _id;
        internal EntityID(ulong ID)
        {
            _id = ID;
        }

        public int CompareTo(EntityID other)
        {
            return _id.CompareTo(other._id);
        }

        public bool Equals(EntityID x, EntityID y)
        {
            return x._id.Equals(y._id);
        }

        public int GetHashCode(EntityID obj)
        {
            return obj._id.GetHashCode();
        }

        public override string ToString()
        {
            return _id.ToString();
        }
    }
}
