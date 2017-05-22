using UnityEngine;
using System;

[Serializable]
public class GridNode2D : SettlersEngine.IPathNode<System.Object>
{
    public Int32 X;
    public Int32 Y;
    public Boolean IsWall;
    
    public GridNode2D(Int32 X,Int32 Y, bool IsWall){
        this.X = X;
        this.Y = Y;
        this.IsWall = IsWall;
    }
    
    public bool IsWalkable(System.Object unused)
    {
        return !IsWall;
    }
}
