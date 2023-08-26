using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
    Stop,
    Walk,
}

/// <summary>
/// A星格子类
/// </summary>
public class AStarNode 
{
    public float f;
    public float g;
    public float h;
    public AStarNode father;
    public int x;
    public int y;
    public E_Node_Type type;

    public AStarNode(int x, int y,E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

}
