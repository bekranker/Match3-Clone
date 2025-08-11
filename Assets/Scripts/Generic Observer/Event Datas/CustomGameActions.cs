using UnityEngine;

public struct OnClick
{
    public Vector2Int Position;
    public Node[,] Cells;
    public OnClick(Vector2Int position, Node[,] cells)
    {
        this.Position = position;
        this.Cells = cells;
    }
}
public struct OnGridInitialized
{
    public Vector2Int GridSize;
    public Node[,] Cells;
    public Vector2Int StartPoint;
    public OnGridInitialized(Vector2Int gridSize, Node[,] cells, Vector2Int startPoint)
    {
        this.GridSize = gridSize;
        this.Cells = cells;
        this.StartPoint = startPoint;
    }

}