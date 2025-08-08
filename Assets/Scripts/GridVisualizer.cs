using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GridVisualizer : MonoBehaviour, IInitializable
{
    [Header("---DOTween Poprs")]
    [SerializeField] private float _popDuration;
    [SerializeField] private float _popStrengthMult;

    [Header("---Grid Values")]
    [SerializeField] private Transform _offset;

    [Header("---Grid Attachments")]
    [SerializeField] private List<GameObject> _nodePrefab;    // 1-)Red, 2- Green, 3-) Blue, 4-) Yellow, 5-) Puprle, 6-) White
    [SerializeField] private GameObject _gridPrefab;

    public Vector2Int Size => _gridManager.GridSize;
    public Vector2 OffsetPosition => _offset.position;

    [Inject] private GridManager _gridManager;
    [Inject] private MatchManager _matchManager;



    /// <summary>
    /// Creating the Object
    /// </summary>
    /// <param name="x">row index</param>
    /// <param name="y">column index</param>
    /// <returns>New grid object</returns>
    public GameObject CreateGrid(int x, int y) => Instantiate(_gridPrefab, ConfigurePosition(x, y), Quaternion.identity);
    /// <summary>
    /// configuring the created node position
    /// </summary>
    /// <param name="x">row index</param>
    /// <param name="y">column index</param>
    /// <returns>the calculated new position</returns>
    private Vector2 ConfigurePosition(int x, int y)
    {
        Vector2 newPos = Vector2.zero;
        newPos.x = x + _offset.position.x - (Size.x / 2f) + .5f;
        newPos.y = y + _offset.position.y - (Size.y / 2f) + .5f;
        return newPos;
    }
    public void SetColor(Node node)
    {
        NodeColor[] nodeColors = (NodeColor[])Enum.GetValues(typeof(NodeColor));
        int randomColorIndex = Random.Range(0, nodeColors.Length - 1);
        NodeColor randomColor = nodeColors[randomColorIndex];
        node.NodeColorValue = randomColor;
        node.NodeObject = CreateNode(node, _nodePrefab[randomColorIndex]);
    }
    public void Initialize()
    {
        EventManager.Subscribe<OnClick>(PopNodes);
    }
    void OnDestroy()
    {
        EventManager.UnSubscribe<OnClick>(PopNodes);
    }
    public void PopNodes(OnClick data) => StartCoroutine(PopNodesIE(data));
    private IEnumerator PopNodesIE(OnClick data)
    {
        List<Node> matched = _matchManager.MatchedPieces(Vector2Int.RoundToInt(data.Position));
        if (matched == null) yield break;
        if (matched.Count <= 1) yield break;

        foreach (Node piece in matched)
        {
            piece.Busy = true;
            yield return piece.NodeObject.transform.DOPunchScale(Vector2.one * _popStrengthMult, _popDuration).WaitForCompletion();
        }
        matched.ForEach((node) => Destroy(node.NodeObject));
    }
    private GameObject CreateNode(Node node, GameObject nodePrefab) => Instantiate(nodePrefab, node.GridObject.transform.position, Quaternion.identity);


}