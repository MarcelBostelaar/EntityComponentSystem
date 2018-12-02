use std::hash::Hash;

pub trait AbstractComponentList<IdType : Eq + Hash>{
    fn getIntersections(&self) -> Vec<IdType>;
}

pub trait ComponentList<IdType, ComponentType>{
    
}

pub trait Component{

}

pub trait System{}