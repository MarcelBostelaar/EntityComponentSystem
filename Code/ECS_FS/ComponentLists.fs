module ComponentLists

open System.Collections.Generic

type ComponentListBuilder<'EntityID, 'Component when 'EntityID : equality>(items : Lazy<seq<'EntityID * 'Component>>)=
    member private this.sequence = items
    member this.Add item = this.sequence.

type ComponentList<'EntityID, 'Component when 'EntityID : equality>(items : seq<'EntityID * 'Component>)= 
    member private this.Components = dict items
    member this.GetComponent ID = if this.Components.ContainsKey ID then Some (this.Components.Item ID) else None
    member 