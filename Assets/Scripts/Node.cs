using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base Node Class for every cell in the grid.
/// </summary>
public class Node
{
    public NodeType NodeTypeValue = NodeType.NORMAL;
    public NodeColor NodeColorValue;
    public string Name;
    public Vector2Int Position;
    public GameObject GridObject;
    public GameObject NodeObject;
    public bool Busy;
    public List<Vector2Int> Neighboors = new();
    public Node(Vector2Int pos, GameObject gridObject, bool isBusy, List<Vector2Int> neighboors)
    {
        Position = pos;
        GridObject = gridObject;
        Busy = isBusy;
        Neighboors = neighboors;
        Name = "Node: " + Position.ToString();
    }
}