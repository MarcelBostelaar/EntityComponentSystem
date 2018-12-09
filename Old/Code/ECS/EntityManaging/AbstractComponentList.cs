
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.EntityManaging
{
    /// <summary>
    /// Contains all the non-generic operations of the component list.
    /// Exists to allow the operation on collections of component lists while also fasciliating internal methods.
    /// </summary>
    abstract public class AbstractComponentList<EntityID, CacheID>
    {
        abstract public IEnumerable<EntityID> GetAllIDs();

        abstract public bool ContainsID(EntityID EntityID);

        abstract internal ulong ID { get;}

        abstract internal bool RemoveComponent(EntityID EntityID);

        abstract internal IReadOnlyCollection<string> GetTags();

        abstract internal bool RemoveCache(CacheID cacheID);

        abstract internal void CreateFromCache(CacheID cacheID, EntityID toEntityID, EntityManager<EntityID, CacheID> manager);
    }
}
