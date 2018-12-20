module klad_0_0_to_example

open RecordTransform
//example generated module

let PointXTransform = RenameEntry "x" "X position"
let PointYTransform = RenameEntry "y" "Y position"

let PointTransform = PointXTransform >> PointYTransform

let threepointtransform = 
    "PointA" @ PointTransform >>
    "PointB" @ PointTransform >> 
    "PointC" @ PointTransform

let superparser = "Point" @ PointTransform >> "ThreePoint" @ threepointtransform