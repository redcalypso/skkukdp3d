using UnityEngine;
using System.Collections.Generic;

public class Manager_ObjectPool : MonoBehaviour
{
    public static Manager_ObjectPool Instance;
    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject Get(string tag, Vector3 position)
    {
        if (!pools.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool for {tag} doesn't exist!");
            return null;
        }
        return pools[tag].Get(position);
    }

    public void Return(string tag, GameObject obj)
    {
        if (pools.ContainsKey(tag))
        {
            pools[tag].Return(obj);
        }
    }

    public void CreatePool(GameObject prefab, int size)
    {
        string tag = prefab.name;
        if (!pools.ContainsKey(tag))
        {
            pools[tag] = new ObjectPool(prefab, size, transform);
        }
    }
} 