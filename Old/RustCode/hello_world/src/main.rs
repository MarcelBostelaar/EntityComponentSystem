mod ComponentList;
mod ECS;

use std::mem::transmute;
use ComponentList::{ComponentList as Componentlist, AbstractComponentList};


struct TestStruct{
    x: Box<str>,
    y: i32,
}

struct oneint{
    x: i32
}

trait TestTrait{
    fn to_string(&self) -> &str;
}

impl TestTrait for TestStruct{
    fn to_string(&self) -> &str{return self.x.as_ref()}
}

fn main() {
    println!("Hello, world!");
    let mut lala = [1, 3, 5];
    lala = [2,4,5];
    //test
    /*tesssest */
    let sometuple = ("hello", 123, 9.5, true);

    let newpoint = TestStruct{x:String::into_boxed_str("Hello".to_string()), y:56};
    let i = getstring(&newpoint);
    let j = getstring(&newpoint);


    let normallist : Componentlist<i32, bool> = Componentlist::new();
    let option = normallist.GetComponent(1);
    match option {
        Some(ref p) => println!("has value {}", p),
        None => println!("has no value"),
    }
    let mut mutlist : Componentlist<i32, bool> = Componentlist::new();
    mutlist.AttachComponent(1, false);
    let option2 = mutlist.GetComponent(1);
    match option2 {
        Some(ref p) => println!("has value {}", p),
        None => println!("has no value"),
    }

    let mut temp : Box<AbstractComponentList<i32>> = Box::new(Componentlist::<i32,bool>::new());
    temp.RemoveComponent(1);

    let ptr = &mut 0;
    let val_transmuted = unsafe {
        std::mem::transmute::<&mut i32, &mut u32>(ptr)
    };

    let teststruct = oneint{x:1};
    let i = &teststruct as *const oneint as *mut TestStruct;

    // Now, put together `as` and reborrowing - note the chaining of `as`
    // `as` is not transitive
    let val_casts = unsafe { &mut *(ptr as *mut i32 as *mut TestStruct) };
}

fn getstring<T: TestTrait>(t: &T) -> &str{
    return t.to_string();
}
