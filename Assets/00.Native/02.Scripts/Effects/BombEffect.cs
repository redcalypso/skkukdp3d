using UnityEngine;
using System.Collections;

public class BombEffect : MonoBehaviour
{
    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    
    [Header("Effect Settings")]
    public float destroyDelay = 2f;
    
    private ParticleSystem _particleSystem;

    private void Start()
    {

        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem != null) _particleSystem.Play();
        
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
} 