using UnityEngine;

public class WorldLayerManager : Singleton<WorldLayerManager>
{
    [SerializeField] private LayerMask _obstacleMask;

    public LayerMask ObstacleMask => _obstacleMask;
}
