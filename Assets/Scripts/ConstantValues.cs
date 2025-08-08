using UnityEngine;

public static class ConstantValues
{
    public static readonly Vector2Int[] Directions =
    {
        new Vector2Int(-1, 0), // LEFT
        new Vector2Int(1, 0), // RIGHT
        new Vector2Int(0, 1), // UP
        new Vector2Int(0, -1), // DOWN
    };
}