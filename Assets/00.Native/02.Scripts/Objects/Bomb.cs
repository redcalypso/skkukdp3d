using UnityEngine;

public class Bomb : MonoBehaviour
{   
    [SerializeField] private GameObject _bombEffectPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_bombEffectPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
