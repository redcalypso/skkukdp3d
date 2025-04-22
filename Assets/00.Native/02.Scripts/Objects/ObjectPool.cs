using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private GameObject _prefab;
    private Transform _parent;
    private Queue<GameObject> _objectPool = new Queue<GameObject>();
    private bool _hasParticleSystem;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
        _hasParticleSystem = prefab.GetComponent<ParticleSystem>() != null;
        
        for (int i = 0; i < initialSize; i++)
        {
            CreatePoolObject();
        }
    }

    private void CreatePoolObject()
    {
        var newObject = Object.Instantiate(_prefab, _parent);
        newObject.SetActive(false);
        _objectPool.Enqueue(newObject);
    }

    public GameObject Get(Vector3 spawnPosition)
    {
        if (_objectPool.Count == 0)
        {
            CreatePoolObject();
            Debug.Log($"Created new object in pool for {_prefab.name}");
        }
        
        var poolObject = _objectPool.Dequeue();
        if (poolObject == null)
        {
            Debug.LogError($"Null object in pool for {_prefab.name}");
            return null;
        }

        poolObject.transform.position = spawnPosition;
        
        if (_hasParticleSystem)
        {
            var particleSystem = poolObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Clear();
                particleSystem.Stop();
                var mainModule = particleSystem.main;
                mainModule.stopAction = ParticleSystemStopAction.Callback;
            }
        }
        
        poolObject.SetActive(true);
        return poolObject;
    }

    public void Return(GameObject poolObject)
    {
        if (_hasParticleSystem)
        {
            var particleSystem = poolObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                particleSystem.Clear();
            }
        }
        
        poolObject.SetActive(false);
        _objectPool.Enqueue(poolObject);
    }
} 