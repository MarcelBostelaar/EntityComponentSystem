use std::collections::HashMap;
use std::hash::Hash;

pub trait AbstractComponentList<IdType>{
    fn RemoveComponent(&mut self, ID:IdType);
}

pub struct ComponentList<IdType, ComponentType>{
    components: HashMap<IdType, ComponentType>,
}
impl <IdType : Eq + Hash, ComponentType> ComponentList<IdType, ComponentType>{
    pub fn new() -> ComponentList<IdType,ComponentType> { ComponentList{components:HashMap::new()} }
    pub fn AttachComponent(&mut self, ID:IdType, Component:ComponentType) {self.components.insert(ID, Component);}
    pub fn GetComponent(&self, ID:IdType) -> Option<&ComponentType>{self.components.get(&ID)}
}

impl <IdType : Eq + Hash, ComponentType> AbstractComponentList<IdType> for ComponentList<IdType, ComponentType>{
    fn RemoveComponent(&mut self, ID:IdType){self.components.remove(&ID);}
}