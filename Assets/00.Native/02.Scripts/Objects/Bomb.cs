using UnityEngine;

public class Bomb : MonoBehaviour
{   
    public GameObject BombEffectPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(BombEffectPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
