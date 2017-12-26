# Entity-Component-System system
This is a simple entity component system- system written as a class library for .net in C#. It targets .net standard 1.0.

A component must implement the IComponent interface. A component must be a class.

Components are stored in a component list by an ID. A component list is owned and obtained from a manager. Mutating a component list is done through the manager. Mutating a component is done though a the component itself.

Component lists should be kept as a reference by the user outside of the manager, so type information is not lost. Systems should be created to do operations on components. Use the static intersect function of the entity manager class to find all the entity ids that have components in a given set of component lists.

A component list is requested with tags in the form of strings. Upon addition of a component to a list, all components with the same ID and one or more shared tag are removed. This allows for exclusive component groups, such as only allowing one AI per entity.

System calling order, component mutation and component list mutation are **not** threadsafe. It is the responsibility of the user to handle concurrency in a correct manner.
