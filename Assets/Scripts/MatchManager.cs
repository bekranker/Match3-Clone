using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MatchManager : MonoBehaviour, IInitializable
{
    [Inject] private GridManager _gridManager;

    public void Initialize()
    {
        EventManager.Subscribe<OnClick>(HasMatch);

    }
    void OnDestroy()
    {
        EventManager.UnSubscribe<OnClick>(HasMatch);
    }
    public List<Node> MatchedPieces(Vector2Int startPoint)
    {
        if (startPoint.x < 0 || startPoint.y < 0 || startPoint.x > _gridManager.GridSize.x || startPoint.y > _gridManager.GridSize.y) return null;
        Node startNode = _gridManager.Cells[startPoint.x, startPoint.y];
        HashSet<Node> visited = new();
        List<Node> matches = new();

        SearchRecursive(startNode, startNode.NodeColorValue, ref visited, matches);
        return matches;
    }

    private void SearchRecursive(Node current, NodeColor targetColor, ref HashSet<Node> visited, List<Node> matches)
    {
        if (visited.Contains(current))
            return;

        visited.Add(current);

        if (current.NodeColorValue != targetColor)
            return;

        matches.Add(current);

        foreach (Vector2Int neighborPos in current.Neighboors)
        {
            Node neighbor = _gridManager.Cells[neighborPos.x, neighborPos.y];
            SearchRecursive(neighbor, targetColor, ref visited, matches);
        }
    }
    public void HasMatch(OnClick data)
    {
        List<Node> matched = MatchedPieces(Vector2Int.RoundToInt(data.Position));
        if (matched == null)
        {
            print("xxxx OUT OF GRID xxxx");
            return;
        }
        if (matched.Count >= 2)
            print($"||||| {matched.Count} MATCH |||||");
        else
            print("xxxx NO MATCHES xxxx");
    }


}