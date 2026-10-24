using UnityEngine;

public class CometSpawnMarker : SpawnMarker
{
    public override PropertyName id => new("Comet Spawn");

    [Header("Parameters")]
    private GameObject targetPrefab;
    public Vector2 position;
    public Vector2 target;
    public float speed;
    public Comet.Pattern pattern;

    public GameObject GetPrefab()
    {
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("Mechanics/Obstacles/Comet Variant");
        }
        return prefab;
    }

    public GameObject GetTargetPrefab()
    {
        if (targetPrefab == null)
        {
            targetPrefab = Resources.Load<GameObject>("Mechanics/Obstacles/Comet Target Variant");
        }
        return targetPrefab;
    }


}