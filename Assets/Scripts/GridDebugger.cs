using UnityEngine;
using Zenject;

public class GridDebugger : MonoBehaviour
{
    [Header("---Props")]
    [SerializeField] private bool _debug;

    [Inject] private GridManager _gridManager;


    public void PrintAllNeightboors(Vector2Int startPoint)
    {
        Node selectedNode = _gridManager.GetSelectedNode(startPoint);
        foreach (Vector2Int neighboorNode in selectedNode.Neighboors)
        {
            print("Coordinates: " + neighboorNode + " |||| " + _gridManager.GetSelectedNode(neighboorNode).Name);
        }
    }
}