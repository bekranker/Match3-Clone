using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    [Header("---Components")]
    [SerializeField] private GridDebugger _gridDebugger;
    [SerializeField] private GridVisualizer _gridVisualizer;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private MatchManager _matchManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MatchManager>().FromInstance(_matchManager).AsCached();
        Container.BindInterfacesAndSelfTo<GridManager>().FromInstance(_gridManager).AsCached();
        Container.BindInterfacesAndSelfTo<GridVisualizer>().FromInstance(_gridVisualizer).AsCached();
        Container.Bind<GridDebugger>().FromInstance(_gridDebugger).AsCached();
    }
}