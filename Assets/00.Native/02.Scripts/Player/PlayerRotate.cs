using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    
    private void Start()
    {
        if (_cameraTransform == null) _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float cameraYRotation = _cameraTransform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0, cameraYRotation, 0);
    }
}