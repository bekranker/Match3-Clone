using UnityEngine;
using Zenject;
using DG.Tweening;
public class NodeHandler : MonoBehaviour, IInitializable
{
    [Inject] GridManager _gridManager;
    [Header("---DoTween Props")]
    [SerializeField] private Ease _ease;
    [SerializeField] private float _duration;
    public void Initialize()
    {
    }

    /// <summary>
    /// a Recursive function that move all pieces if space is null
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Tween MoveNode(Node node)
    {
        Node underNode = _gridManager.GetSelectedNode(node.Position - Vector2Int.down);
        Node aboveNode = _gridManager.GetSelectedNode(node.Position - Vector2Int.up);
        bool isUnderEmpty = underNode.NodeObject == null ? true : false;
        if (!isUnderEmpty) return null;
        node.NodeObject.transform.DOMoveY(underNode.Position.y, _duration).SetEase(_ease);
        // node.NodeObject
        return MoveNode(aboveNode);
    }
}
