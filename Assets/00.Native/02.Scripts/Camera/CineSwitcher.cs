using UnityEngine;
using Unity.Cinemachine;

public class CineSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cams")]
    public CinemachineCamera fpsCam;
    public CinemachineCamera tpsCam;
    public CinemachineCamera quarterCam;

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
        fpsCam.Priority = lowPriority;
        tpsCam.Priority = lowPriority;
        quarterCam.Priority = lowPriority;

        switch (view)
        {
            case CameraView.FPSView:
                fpsCam.Priority = highPriority;
                break;
            case CameraView.TPSView:
                tpsCam.Priority = highPriority;
                break;
            case CameraView.QuaterView:
                quarterCam.Priority = highPriority;
                break;
        }
    }
}