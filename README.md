# Entity-Component-System system
This is a simple entity component system- system written as a class library for .net in C#. It targets .net standard 1.0.

A component must implement the IComponent interface. A component must be a class.

Components are stored in a component list by an ID. A component list is owned and obtained from a manager. An entity ID is obtained from a managar. Mutating a component list is done through the manager. Mutating a component is done though a the component itself.

Component lists should be kept as a reference by the user outside of the manager, so type information is not lost. Systems should be created to do operations of components. Use the static intersect function of the entity manager class to find all the entity ids that have components in a given set of component lists.

System calling order, component mutation and component list mutation are **not** threadsafe. It is the responsibility of the user to handle concurrency in a correct manner.