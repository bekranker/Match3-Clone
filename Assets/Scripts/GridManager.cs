using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class GridManager : MonoBehaviour, IInitializable
{
    [Inject] private GridVisualizer _gridVisualizer;
    [Header("---Grid Props")]
    [SerializeField] private Vector2Int _size;
    public Vector2Int GridSize => _size;
    [SerializeField] private float _spawnRate;
    [SerializeField] private Vector2Int _clickPoint;

    public Node[,] Cells;

    public void Initialize()
    {
        StartCreate();
    }
    void StartCreate() => StartCoroutine(CreateGrid());
    /// <summary>
    /// Creating the Nodes with _size varabile
    /// </summary>
    /// <returns>IEnumerator returning a Coroutine</returns>
    IEnumerator CreateGrid()
    {
        Cells = new Node[_size.x, _size.y];
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                Node newCell = new Node(new Vector2Int(x, y), _gridVisualizer.CreateGrid(x, y), true, SetNeighboors(x, y));
                Cells[x, y] = newCell;
                // _gridVisualizer.SetColor(newCell);
                yield return new WaitForSeconds(_spawnRate);
            }
        }
        SetFreeAllGrid();
        for (int x = 0; x < GridSize.x; x++)
        {
            EventManager.Raise(new OnGridInitialized(GridSize, Cells, new Vector2Int(x, 0)));
        }
    }
    /// <summary>
    /// Configure the neighboors
    /// </summary>
    /// <param name="x">Row index</param>
    /// <param name="y">Column index</param>
    /// <returns>All neighboor's array positions</returns>
    private List<Vector2Int> SetNeighboors(int x, int y)
    {
        List<Vector2Int> newNeighboors = new();
        foreach (Vector2Int dir in ConstantValues.Directions)
        {
            int newX = dir.x + x;
            int newY = dir.y + y;

            if (newX >= 0 && newX < _size.x && newY >= 0 && newY < _size.y)
            {
                newNeighboors.Add(new Vector2Int(newX, newY));
            }
        }
        return newNeighboors;
    }
    /// <summary>
    /// All nodes turning to Free (bool Busy = true)
    /// </summary>
    void SetFreeAllGrid()
    {
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                Cells[x, y].Busy = false;
            }
        }
    }
    /// <summary>
    /// searching the clicked node
    /// </summary>
    /// <param name="input">array position</param>
    /// <returns>returning the selected piece from Cells[,] 2D array</returns>
    public Node GetSelectedNode(Vector2Int input)
    {
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                if (Cells[x, y].Position == input)
                    return Cells[x, y];
            }
        }
        return null;
    }
}