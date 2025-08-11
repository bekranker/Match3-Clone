using UnityEngine;
using Zenject;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using Unity.Collections;
public class NodeHandler : MonoBehaviour, IInitializable
{
    [Inject] GridManager _gridManager;
    [Inject] private MatchManager _matchManager;

    [Header("---DoTween Props")]
    [SerializeField] private Ease _ease;
    [SerializeField] private float _duration;
    [SerializeField] private Vector2 _popStrengthMult;
    [SerializeField] private float _popDuration;

    [Header("---Grid Attachments")]
    [SerializeField] private List<GameObject> _nodePrefab;    // 1-)Red, 2- Green, 3-) Blue, 4-) Yellow, 5-) Puprle, 6-) White

    public void Initialize()
    {
        //EventManager.Subscribe<OnGridInitialized>(InitializeNodes);
        EventManager.Subscribe<OnClick>(PopNodes);
    }
    public void OnDestroy()
    {
        //EventManager.UnSubscribe<OnGridInitialized>(InitializeNodes);
        EventManager.UnSubscribe<OnClick>(PopNodes);
    }
    public int SetColor(Node node)
    {
        NodeColor[] nodeColors = (NodeColor[])Enum.GetValues(typeof(NodeColor));
        int randomColorIndex = Random.Range(0, nodeColors.Length - 1);
        NodeColor randomColor = nodeColors[randomColorIndex];
        node.NodeColorValue = randomColor;
        return randomColorIndex;
    }
    public void PopNodes(OnClick data) => StartCoroutine(PopNodesIE(data));
    private IEnumerator PopNodesIE(OnClick data)
    {
        if (data.Cells[data.Position.x, data.Position.y].NodeObject == null) yield break;
        List<Node> matched = _matchManager.MatchedPieces(data.Position);
        if (matched == null) yield break;
        if (matched.Count <= 1) yield break;

        foreach (Node piece in matched)
        {
            piece.Busy = true;
            yield return piece.NodeObject.transform.DOPunchScale(Vector2.one * _popStrengthMult, _popDuration).WaitForCompletion();
        }
        matched.ForEach((node) => Destroy(node.NodeObject));
    }
    public GameObject CreateNode(Node node)
    {
        int nodePrefabIndex = SetColor(node);
        return Instantiate(_nodePrefab[nodePrefabIndex], new Vector2(node.Position.x, _gridManager.GridSize.y + 1), Quaternion.identity);
    }

    /// <summary>
    /// a Recursive function that move all pieces if space is null
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Tween MoveNode(Node node, Vector2Int to)
    {
        node.Busy = true;
        return node.NodeObject.transform.DOMove((Vector2)to, _duration).SetEase(_ease).OnComplete(() => node.Busy = false);
    }
    public void InitializeNodes(OnGridInitialized data)
    {
        Node[,] cells = data.Cells;
        Vector2Int startPoint = data.StartPoint;


        for (int y = startPoint.y; y < data.GridSize.y; y++)
        {
            Node currentNode = cells[startPoint.x, y];
            if (cells[startPoint.x, y].NodeObject != null) return;
            if (y + 1 >= data.GridSize.y)
            {
                //Create new Node and take it
                Node newNodeData = new Node(new Vector2Int(startPoint.x, y), currentNode.GridObject, false, currentNode.Neighboors);
                CreateNode(newNodeData);
                MoveNode(newNodeData, newNodeData.Position);
                _gridManager.Cells[startPoint.x, y] = newNodeData;
                return;
            }
            if (cells[startPoint.x, y + 1].NodeObject != null)
            {
                Node aboveNode = cells[startPoint.x, y + 1];
                //Take the node from cell at the above
                SwitchNodes(currentNode, aboveNode);
                _gridManager.Cells[startPoint.x, y] = aboveNode;
                MoveNode(currentNode, aboveNode.Position);
            }
            else
            {
                //Above cell has no Node inside.
                //we have to check all cells above us.
                //if any cells above us has any match, take it and call same function for above cell.
                bool isColumnEmpty = true;
                for (int i = startPoint.y; i < data.GridSize.y; i++)
                {
                    Node aboveNode = cells[startPoint.x, i];
                    if (aboveNode.NodeObject != null)
                    {
                        isColumnEmpty = false;
                        return;
                    }
                }
                if (isColumnEmpty)
                {
                    //Create new Node and take it
                    Node newNodeData = new Node(new Vector2Int(startPoint.x, y), currentNode.GridObject, false, currentNode.Neighboors);
                    CreateNode(newNodeData);
                    MoveNode(newNodeData, newNodeData.Position);
                    _gridManager.Cells[startPoint.x, y] = newNodeData;
                    return;
                }

            }
        }
    }
    private Node SwitchNodes(Node from, Node to)
    {
        from.GridObject = to.GridObject;
        from.NodeObject = to.NodeObject;
        from.Name = to.Name;
        from.Neighboors = to.Neighboors;
        from.NodeColorValue = to.NodeColorValue;
        from.Position = to.Position;
        from.NodeTypeValue = to.NodeTypeValue;

        return from;
    }
}