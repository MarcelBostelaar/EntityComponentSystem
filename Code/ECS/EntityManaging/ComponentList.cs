using System;
using System.Collections.Generic;
using System.Text;
using ECS.IDs;
using ECS.Exceptions;
using ECS.Components;

namespace ECS.EntityManaging
{
    public class ComponentList<T> : AbstractComponentList where T: class, IComponent
    {
        Dictionary<EntityID, T> components = new Dictionary<EntityID, T>();
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
        /// <param name="ID">The entity ID</param>
        /// <returns>The component</returns>
        /// <exception cref="ItemDoesntExistException">Thrown when the list does not contain a component of the given ID.</exception>
        public T GetComponent(EntityID ID)
        {
            if (components.ContainsKey(ID))
            {
                return components[ID];
            }
            throw new ItemDoesntExistException(string.Format("Entity '{0}' does not contain a component of type '{1}'", ID.ToString(), typeof(T).FullName));
        }
        
        /// <summary>
        /// Gets all the components of the ids.
        /// </summary>
        /// <param name="IDs">An enumerable of entity IDs</param>
        /// <returns>A list of tuples with the id and the component.</returns>
        /// <exception cref="ItemDoesntExistException">Thrown when the list does not contain a component of a given ID.</exception>
        public List<Tuple<EntityID, T>> GetComponents(IEnumerable<EntityID> IDs)
        {
            var newlist = new List<Tuple<EntityID, T>>();
            foreach (var id in IDs)
            {
                newlist.Add(new Tuple<EntityID, T>(id, GetComponent(id)));
            }
            return newlist;
        }

        /// <summary>
        /// Attaches a component to the list. Replaces the component of the ID if it already was in place.
        /// </summary>
        /// <param name="ID">The ID to attach the component to.</param>
        /// <param name="component">The component to attach.</param>
        internal void AttachComponent(EntityID ID, T component)
        {
            components[ID] = component;
        }

        /// <summary>
        /// Removes the component of a specified ID. Explicitly implemented so that it is internal.
        /// </summary>
        /// <param name="ID">The entity ID of the component to remove</param>
        /// <returns>True if the component existed, false if the id did not have a component in this list.</returns>
        internal override bool RemoveComponent(EntityID ID)
        {
            return components.Remove(ID);
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
        /// <param name="id">The ID to check for.</param>
        /// <returns>Bolean indicating if it contains or does not contain a component for that id.</returns>
        public override bool ContainsID(EntityID id)
        {
            return components.ContainsKey(id);
        }

        /// <summary>
        /// Returns the tags registered to this list. Implemented explicitly to keep method internal.
        /// </summary>
        /// <returns>An Enumerable of tags (strings)</returns>
        internal override IReadOnlyCollection<string> GetTags()
        {
            return Tags;
        }
    }
}
