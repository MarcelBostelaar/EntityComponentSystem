use ECS::Traits::{AbstractComponentList, ComponentList, Component, System};
use std::any::{Any, TypeId};
use std::collections::HashMap;
use std::hash::Hash;
use std::result;

struct ComponentManager<IdType>{
    component_lists: Vec<Box<AbstractComponentList<IdType>>>,
}
struct SimpleComponentHashmapList<IdType, ComponentType>{
    components: HashMap<IdType, ComponentType>,
}
impl<IdType, ComponentType> ComponentList<IdType, ComponentType> for SimpleComponentHashmapList<IdType, ComponentType>{

}
impl<IdType: Eq + Hash, ComponentType> AbstractComponentList<IdType> for SimpleComponentHashmapList<IdType, ComponentType>{
    fn getIntersections(&self) -> Vec<IdType>{
        return Vec::new()
    }
}
struct ComponentManagerBuilder<IdType>{
    // component_lists: HashMap<TypeId, fn()->Box<AbstractComponentList<IdType>>>,
    // component_lists_typed: HashMap<TypeId, fn()->
    //component lists in builder should save methods that produce a lists saved by type
    //system list buildfunctions should contain functions that take a list of functions that take a hasmap of abstractlists by type, and return a build system or an error
    component_lists: HashMap<TypeId, fn()->Box<AbstractComponentList<IdType>>>,
    system_list_buildfunctions: Vec<fn(HashMap<TypeId, Box<AbstractComponentList<IdType>>>) -> Result<Box<System>, String>>,
}

impl<IdType: Eq + Hash + 'static> ComponentManagerBuilder<IdType>{
    fn new() -> Result<ComponentManagerBuilder<IdType>, String>{
        return Ok(ComponentManagerBuilder{
            component_lists: HashMap::<TypeId, fn()->Box<AbstractComponentList<IdType>>>::new(),
            system_list_buildfunctions: Vec::<fn(HashMap<TypeId, Box<AbstractComponentList<IdType>>>) -> Result<Box<System>, String>>::new(),
            })
        }
    fn with_component_type<ComponentType : Component + 'static>(mut self) -> Result<ComponentManagerBuilder<IdType>,String>{
        if self.component_lists.contains_key(&TypeId::of::<ComponentType>()){
            return Err(String::from("Could not register a component in 'with_component', as it already is registered."));
        }
        else{
            self.component_lists.insert(TypeId::of::<ComponentType>(), || Box::new(SimpleComponentHashmapList{components:HashMap::<IdType, ComponentType>::new()}));
            return Ok(self);
        }
    }
    // fn with_system<T1>(mut self, In:(IdType,T1)) -> Result<ComponentManagerBuilder<IdType>,String> where T1 : Component + 'static{

    // }
    // fn build(&self) -> ComponentManager<IdType>{

    // }
}



//shorthand methods
trait CMBShorthands<IdType>{
    fn with_component_type<ComponentType : Component + 'static>(self) -> Result<ComponentManagerBuilder<IdType>,String>;
}
impl<IdType: Eq + Hash + 'static> CMBShorthands<IdType> for Result<ComponentManagerBuilder<IdType>,String>{
    /// Shorthand for self::and_then(ComponentManagerBuilder::with_component_type)
    fn with_component_type<ComponentType : Component + 'static>(self) -> Result<ComponentManagerBuilder<IdType>,String>{
        Result::and_then(self, ComponentManagerBuilder::with_component_type::<ComponentType>)
    }
}