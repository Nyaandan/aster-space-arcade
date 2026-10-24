using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    ASTEROID, COMET, COMET_TARGET, METEOR, GAMMA_RAY
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Serializable] public class ObjectPool
    {
        public ObjectType type;
        public GameObject prefab;
        public int amount;
    }

    [SerializeField] ObjectPool[] objectPools;
    Dictionary<ObjectType, List<GameObject>> pools;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        pools = new Dictionary<ObjectType, List<GameObject>>(objectPools.Length);
        foreach (var pool in objectPools)
        {
            List<GameObject> list = new(pool.amount);
            GameObject obj;
            for (int i = 0, n = pool.amount; i < n; i++)
            {
                obj = Instantiate(pool.prefab, transform);
                obj.name = pool.prefab.name + " " + i;
                obj.SetActive(false);
                list.Add(obj);
            }
            pools[pool.type] = list;
        }
    }

    // Overloaded method to get pooled object by int code instead of ObjectType enum name
    public GameObject GetPooledObject(int code) => GetPooledObject((ObjectType)code);

    public GameObject GetPooledObject(ObjectType type)
    {
        if (!pools.TryGetValue(type, out var pool)) return null;

        for (int i = 0, n = pool.Count; i < n; i++)
        {
            var o = pool[i];
            if (!o.activeSelf) return o;
        }

        return null;
    }

    public void PauseObjects()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeInHierarchy)
                //child.GetComponent<ParkourObject>().StopAdvance();
                print("Object paused: " + child.name);
        }
    }

    public void DisableObjects()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(false);
        }
    }
}
