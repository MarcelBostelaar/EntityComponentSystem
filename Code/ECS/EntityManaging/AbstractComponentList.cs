
using System;
using System.Collections.Generic;
using System.Text;
using ECS.IDs;

namespace ECS.EntityManaging
{
    /// <summary>
    /// Contains all the non-generic operations of the component list.
    /// Exists to allow the operation on collections of component lists while also fasciliating internal methods.
    /// </summary>
    abstract public class AbstractComponentList
    {
        abstract public IEnumerable<EntityID> GetAllIDs();

        abstract public bool ContainsID(EntityID id);

        abstract internal ulong ID { get;}

        abstract internal bool RemoveComponent(EntityID ID);

        abstract internal IReadOnlyCollection<string> GetTags();
    }
}
