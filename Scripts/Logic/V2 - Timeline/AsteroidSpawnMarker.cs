using UnityEngine;

public class AsteroidSpawnMarker : SpawnMarker
{
    public override PropertyName id => new("Asteroid Spawn");

    [Header("Parameters")]
    public Vector2 position;
    public Vector2 direction;
    public float speed;

    public GameObject GetPrefab()
    {
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("Mechanics/Obstacles/Asteroid Variant");
        }
        return prefab;
    }
}