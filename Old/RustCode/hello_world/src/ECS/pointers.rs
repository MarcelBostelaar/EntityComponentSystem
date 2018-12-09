use std::any::{Any, TypeId};

struct Unusable{
}

struct PointerValue<ToType>{
    _value: ToType
}

trait Pointer{
    fn hello(self);
    fn cast_as<to_type>(self) -> Result<to_type, Pointer>;
}

impl <ToType> Pointer for PointerValue<ToType>{
    fn hello(self){}
    fn cast_as<to_type>(self) -> Result<to_type, Pointer>{
        if()
    }
}

impl <ToType: 'static> PointerValue<ToType>{
    fn from_reference(value:ToType) -> PointerValue<ToType>{
        return PointerValue{_value:value};
    }
    fn to_pointer(self) -> Box<Pointer>{
        return Box::new(self);
    }
}

// impl <ToType> Pointer for Pointer<ToType>{
//     fn from_object<object_type>(value: object_type) ->Pointer{
//         return Pointer{type_id:TypeId::of::<object_type>(), value: &value as *const object_type as *const Unusable}
//     }
// }