using System;
using UnityEngine;

public enum CameraView
{
    FPSView,
    TPSView,
    QuaterView
}

public class Manager_CameraInput : MonoBehaviour
{
    public static Manager_CameraInput instance;
    public static Action<CameraView> OnCameraSwitch;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKeyDown(KeyCode.Alpha3)) OnCameraSwitch?.Invoke(CameraView.FPSView);
        else if (!Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKeyDown(KeyCode.Alpha3)) OnCameraSwitch?.Invoke(CameraView.TPSView);
        else if (!Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKeyDown(KeyCode.Alpha3)) OnCameraSwitch?.Invoke(CameraView.QuaterView);
    }
}
