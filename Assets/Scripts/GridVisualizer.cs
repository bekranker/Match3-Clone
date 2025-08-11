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
    [SerializeField] private List<GameObject> _nodePrefab;
    [SerializeField] private GameObject _gridPrefab;

    public Vector2Int Size => _gridManager.GridSize;
    public Vector2 OffsetPosition => _offset.position;

    [Inject] private GridManager _gridManager;



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
    public void Initialize()
    {
    }
}