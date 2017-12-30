using System;
using System.Collections.Generic;
using System.Text;
using ECS.Exceptions;
using ECS.Interfaces;
using ECS.Utilities;

namespace ECS.EntityManaging
{
    public class ComponentList<ComponentType, EntityID, CacheID> : AbstractComponentList<EntityID, CacheID> where ComponentType: class, IComponent
    {
        Dictionary<EntityID, ComponentType> components = new Dictionary<EntityID, ComponentType>();
        IReadOnlyCollection<string> Tags;
        ulong _id;
        
        internal override ulong ID { get { return _id; } }

        /// <summary>
        /// Constructor for a component list
        /// </summary>
        /// <param name="id">A unique ID for this component list for internal identification purposes.</param>
        /// <param name="tagEvents">An enumerable of delegates to call upon </param>
        internal ComponentList(ulong id, IReadOnlyCollection<string> tags)
        {
            this._id = id;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the component of a given ID
        /// </summary>
        /// <param name="EntityID">The entity ID</param>
        /// <returns>The component</returns>
        /// <exception cref="ItemDoesntExistException">Thrown when the list does not contain a component of the given ID.</exception>
        public ComponentType GetComponent(EntityID EntityID)
        {
            if (components.ContainsKey(EntityID))
            {
                return components[EntityID];
            }
            throw new ItemDoesntExistException(string.Format("Entity '{0}' does not contain a component of type '{1}'", EntityID.ToString(), typeof(ComponentType).FullName));
        }

        /// <summary>
        /// Gets all the components of the ids.
        /// </summary>
        /// <param name="EntityIDs">An enumerable of entity IDs</param>
        /// <returns>A list of tuples with the id and the component.</returns>
        /// <exception cref="ItemDoesntExistException">Thrown when the list does not contain a component of a given ID.</exception>
        public List<Tuple<EntityID, ComponentType>> GetComponents(IEnumerable<EntityID> EntityIDs)
        {
            var newlist = new List<Tuple<EntityID, ComponentType>>();
            foreach (var id in EntityIDs)
            {
                newlist.Add(new Tuple<EntityID, ComponentType>(id, GetComponent(id)));
            }
            return newlist;
        }

        /// <summary>
        /// Attaches a component to the list. Replaces the component of the ID if it already was in place.
        /// </summary>
        /// <param name="EntityID">The ID to attach the component to.</param>
        /// <param name="component">The component to attach.</param>
        internal void AttachComponent(EntityID EntityID, ComponentType component)
        {
            components[EntityID] = component;
        }

        /// <summary>
        /// Removes the component of a specified ID. Explicitly implemented so that it is internal.
        /// </summary>
        /// <param name="EntityID">The entity ID of the component to remove</param>
        /// <returns>True if the component existed, false if the id did not have a component in this list.</returns>
        internal override bool RemoveComponent(EntityID EntityID)
        {
            return components.Remove(EntityID);
        }

        /// <summary>
        /// Gets all the entity ids of the components currently in the list
        /// </summary>
        /// <returns>An enumerable of IDs</returns>
        public override IEnumerable<EntityID> GetAllIDs()
        {
            return components.Keys;
        }

        /// <summary>
        /// Returns a boolean indication whether or not the list contains a component of a specific ID.
        /// </summary>
        /// <param name="EntityID">The ID to check for.</param>
        /// <returns>Bolean indicating if it contains or does not contain a component for that id.</returns>
        public override bool ContainsID(EntityID EntityID)
        {
            return components.ContainsKey(EntityID);
        }

        /// <summary>
        /// Returns the tags registered to this list. Implemented explicitly to keep method internal.
        /// </summary>
        /// <returns>An Enumerable of tags (strings)</returns>
        internal override IReadOnlyCollection<string> GetTags()
        {
            return Tags;
        }


        Dictionary<CacheID, Func<Union<ComponentType, None>>> _cache = new Dictionary<CacheID, Func<Union<ComponentType, None>>>();
        internal void AttachCache(Func<Union<ComponentType, None>> componentCreationFunction, CacheID cacheID)
        {
            _cache[cacheID] = componentCreationFunction;
        }

        internal override bool RemoveCache(CacheID cacheID)
        {
            return _cache.Remove(cacheID);
        }

        internal override void CreateFromCache(CacheID cacheID, EntityID toEntityID)
        {
            Func<Union<ComponentType, None>> cached;
            _cache.TryGetValue(cacheID, out cached);
            if(cached == null)
            {
                throw new ItemDoesntExistException(string.Format("Component of type {0} was not cached with id {1}", typeof(ComponentType).FullName, cacheID.ToString()));
            }
            cached().Match<int>(
                createdComponent =>
                {
                    components[toEntityID] = createdComponent;
                    return 0;
                },
                createdNone => 0
                );
        }
    }
}
