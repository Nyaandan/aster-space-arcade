using UnityEngine;

public class GammaRaySpawnMarker : SpawnMarker
{
    public override PropertyName id => new("Gamma Ray Spawn");

    [Header("Parameters")]
    public float distanceToCenter;
    public float orientation;
    public float magnitude;
    public float burstDelay;
    public float burstDuration;

    public GameObject GetPrefab()
    {
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("Mechanics/Obstacles/GammaRay Variant");
        }
        return prefab;
    }
}