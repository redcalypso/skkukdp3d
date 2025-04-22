using UnityEngine;
using System.Collections;

public class BombEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    private float _destroyDelay = 2f;
    
    private ParticleSystem _particleSystem;

    private void Start()
    {

        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem != null) _particleSystem.Play();
        
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(gameObject);
    }
} 