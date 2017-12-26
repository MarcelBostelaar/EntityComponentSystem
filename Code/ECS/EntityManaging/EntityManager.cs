using ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ECS.Exceptions;

namespace ECS.EntityManaging
{
    public class EntityManager<EntityID>
    {
        Dictionary<ulong, AbstractComponentList<EntityID>> components = new Dictionary<ulong, AbstractComponentList<EntityID>>(); //Component lists saved on their ID
        Dictionary<string, List<AbstractComponentList<EntityID>>> componentsByTag = new Dictionary<string, List<AbstractComponentList<EntityID>>>(); //For every tag, the component lists with that tag

        ulong listIDCounter = 1;
        ulong entityIDCounter = 1;

        /// <summary>
        /// Removes all the components of an entity from the system.
        /// </summary>
        /// <param name="ID">The entity identification</param>
        public void RemoveEntity(EntityID ID)
        {
            foreach (var item in components.Values)
            {
                item.RemoveComponent(ID);
            }
        }

        /// <summary>
        /// Attaches a component to an entity and removes all components which share tags.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="componentList">The componentlist to attach to</param>
        /// <param name="component">The component to attach</param>
        /// <param name="id">The entity ID to attach the component to</param>
        public void AttachComponent<T>(ComponentList<T, EntityID> componentList, T component, EntityID id) where T : class, IComponent
        {
            if (!Contains(componentList))
            {
                throw new NotOwnedByManagerException("Component list is not owned by this manager! Manager cannot perform operations on it!");
            }
            var tags = componentList.GetTags();
            foreach (var tag in tags)
            {
                foreach (var list in componentsByTag[tag])
                {
                    list.RemoveComponent(id);
                }
            }
            componentList.AttachComponent(id, component);
        }

        /// <summary>
        /// Removed a component from a component list.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="componentList">The component list to remove the component from</param>
        /// <param name="id">The id of the entity for which to remove the component</param>
        /// <returns>True if the component existed, false if there was no component to begin with.</returns>
        public bool RemoveComponent<T>(ComponentList<T, EntityID> componentList, EntityID id) where T : class, IComponent
        {
            if (Contains(componentList))
            {
                return componentList.RemoveComponent(id);
            }
            throw new NotOwnedByManagerException("Component list is not owned by this manager! Manager cannot perform operations on it!");
        }

        /// <summary>
        /// Creates a new ComponentList for a component of type T and registers given tags to it.
        /// </summary>
        /// <typeparam name="T">The type of the component to make the list ofr.</typeparam>
        /// <param name="tags">The tags to register with this component. Adding an item to this list will remove all components with a shared tag.</param>
        /// <returns>A new component list registered to this entity manager.</returns>
        public ComponentList<T, EntityID> GetNewList<T>(params string[] tags) where T : class, IComponent
        {
            var newlist = new ComponentList<T, EntityID>(listIDCounter, tags);
            listIDCounter++;
            components[newlist.ID] = newlist;
            foreach (var tag in tags)
            {
                if (!componentsByTag.ContainsKey(tag))
                {
                    componentsByTag[tag] = new List<AbstractComponentList<EntityID>>();
                }
                componentsByTag[tag].Add(newlist);
            }
            return newlist;
        }
        
        /// <summary>
        /// Returns a boolean indicating if a component list is owned/contained by this manager.
        /// </summary>
        /// <param name="list">The list to check</param>
        /// <returns>A boolean indicating if it is owned/contained by this manager.</returns>
        private bool Contains(AbstractComponentList<EntityID> list)
        {
            AbstractComponentList<EntityID> listindict;
            components.TryGetValue(list.ID, out listindict);

            return (ReferenceEquals(listindict, list));
        }

        /// <summary>
        /// Returns the ids of entities that have components in all the componentlists.
        /// </summary>
        /// <param name="componentLists">The component lists in which to find the entities</param>
        /// <returns>An enumerable of entity ID's that are in all the lists.</returns>
        public static IEnumerable<EntityID> IntersectForID(params AbstractComponentList<EntityID>[] componentLists)
        {
            if (componentLists.Length == 0)
            {
                return new List<EntityID> { };
            }
            if (componentLists.Length == 1)
            {
                return componentLists[0].GetAllIDs();
            }
            var aslist = componentLists.ToList();
            var intersectedSet = aslist[0].GetAllIDs();
            aslist.RemoveAt(0);
            intersectedSet = aslist.Aggregate(intersectedSet, (intersected, list) =>
            {
                List<EntityID> intersecting = new List<EntityID>();
                foreach (var id in intersected)
                {
                    if (list.ContainsID(id))
                        intersecting.Add(id);
                }
                return intersecting;
            });
            return intersectedSet;
        }

    }
}
