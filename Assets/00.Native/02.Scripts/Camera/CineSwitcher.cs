using UnityEngine;
using Unity.Cinemachine;

public class CineSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cams")]
    [SerializeField] private CinemachineCamera _fpsCam;
    [SerializeField] private CinemachineCamera _tpsCam;
    [SerializeField] private CinemachineCamera _quarterCam;

    [Header("Priority Levels")]
    public int highPriority = 20;
    public int lowPriority = 10;

    private void OnEnable()
    {
        Manager_CameraInput.OnCameraSwitch += HandleCameraSwitch;
    }

    private void OnDisable()
    {
        Manager_CameraInput.OnCameraSwitch -= HandleCameraSwitch;
    }

    private void Start()
    {
        SetPriorities(CameraView.FPSView);
    }

    private void HandleCameraSwitch(CameraView view)
    {
        SetPriorities(view);
    }

    private void SetPriorities(CameraView view)
    {
        _fpsCam.Priority = lowPriority;
        _tpsCam.Priority = lowPriority;
        _quarterCam.Priority = lowPriority;

        switch (view)
        {
            case CameraView.FPSView:
                _fpsCam.Priority = highPriority;
                break;
            case CameraView.TPSView:
                _tpsCam.Priority = highPriority;
                break;
            case CameraView.QuaterView:
                _quarterCam.Priority = highPriority;
                break;
        }
    }
}