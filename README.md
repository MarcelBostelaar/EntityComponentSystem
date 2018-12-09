# Entity-Component-System system
The third attempt at making an entity component system I am happy with.
This time I am making it in mostly in F#, and it works roughly as follows:
All systems generate a change value based on the state given. This can be a command (request) to do some action or a change in value of a component.
All value changes are collected, summed up in case of numerical changes, and then validated, after which they are applied to create a new state.
This project also includes parser tool that can be used to parse data, and a tool to generate code for components based on json.