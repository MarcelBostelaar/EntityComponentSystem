using ECS.Interfaces;
using ECS.EntityManaging;
using System;
using System.Collections.Generic;
using System.Text;
using ECS.Exceptions;

namespace ECS.Deserializer
{
    public class Deserializer<InputType, EntityID, CacheID>
    {
        List<Action<InputType, EntityID>> _deserializeActions = new List<Action<InputType, EntityID>>(); //Deserialize a component and register it to an entityID
        List<Action<InputType, CacheID>> _cacheCreationFunction = new List<Action<InputType, CacheID>>(); //Deserialize a component and register it to a cacheID

        EntityManager<EntityID, CacheID> _manager;
        /// <summary>
        /// Creates a new deserialization object associated with an entity manager. All actions will be performed on this manager.
        /// </summary>
        /// <param name="manager">The entity manager to be associated with.</param>
        public Deserializer(EntityManager<EntityID, CacheID> manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Registers a component creation function to a component list.
        /// </summary>
        /// <typeparam name="ComponentType">The component type to register.</typeparam>
        /// <param name="componentList">The component list to register this function to.</param>
        /// <param name="ComponentCreationFunction">A function that takes an input of type ComponentType and returns a function that creates a completely new component upon being called.</param>
        public void RegisterComponentDeserializer<ComponentType>(ComponentList<ComponentType, EntityID, CacheID> componentList, Func<InputType, Func<ComponentType>> ComponentCreationFunction) where ComponentType : class, IComponent
        {
            if (_manager.Contains(componentList))
            {
                _deserializeActions.Add( 
                    (inputValue, EntityID) => _manager.AttachComponent(componentList, ComponentCreationFunction(inputValue)(), EntityID) //Takes an inputValue and an entity ID and creates a new entity with the deserialized values on that ID
                    );

                _cacheCreationFunction.Add( 
                    (inputValue, cacheID) => _manager.CacheComponentCreation(cacheID, ComponentCreationFunction(inputValue), componentList)); //Takes an inputValue and a cacheID and creates a new cached item on that ID
            }
            else
            {
                throw new NotOwnedByManagerException("This list is not owned by this manager!");
            }
        }

        /// <summary>
        /// Deserializes an input and saves it in at given entityID
        /// </summary>
        /// <param name="value">The value to deserialize</param>
        /// <param name="asEntitytID">The EntityID to store it in in the manager</param>
        public void DeserializeToEntity(InputType value, EntityID asEntitytID)
        {
            foreach (var item in _deserializeActions)
            {
                item(value, asEntitytID);
            }
        }

        /// <summary>
        /// Deserializes an input and saves it in at given cacheID
        /// </summary>
        /// <param name="value">The value to deserialize</param>
        /// <param name="cacheID">The cacheID to store it in in the manager</param>
        public void DeserializeToCache(InputType value, CacheID cacheID)
        {
            foreach (var item in _cacheCreationFunction)
            {
                item(value, cacheID);
            }
        }
    }
}
