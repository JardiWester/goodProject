using UnityEngine;
using UnityEngine.XR;

public class VRHapticFeedback : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Controller Settings")]
    public XRNode leftControllerNode = XRNode.LeftHand;
    public XRNode rightControllerNode = XRNode.RightHand;

    [Header("Camera Settings")]
    public Camera mainCamera;

    [Header("Haptic Settings")]
    public float maxIntensity = 1.0f;    
    public float minIntensity = 0.1f;    
    public float maxScreenDistance = 0.2f; 
    public float hapticDuration = 0.1f;  

    void Update()
    {
        Vector3 screenTargetPosition = mainCamera.WorldToScreenPoint(target.position);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, screenTargetPosition.z);
        float screenDistance = Vector2.Distance(new Vector2(screenTargetPosition.x, screenTargetPosition.y), 
                                                new Vector2(screenCenter.x, screenCenter.y));
        float normalizedDistance = Mathf.Clamp01(1 - (screenDistance / (Screen.width * maxScreenDistance)));
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedDistance);
        Debug.Log($"Crosshair Distance to Target: {screenDistance}, Intensity: {intensity}");
        ApplyHapticFeedback(intensity);
    }

    private void ApplyHapticFeedback(float intensity)
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftControllerNode);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightControllerNode);

        if (leftDevice.isValid)
        {
            SendHapticImpulse(leftDevice, intensity, hapticDuration);
        }

        if (rightDevice.isValid)
        {
            SendHapticImpulse(rightDevice, intensity, hapticDuration);
        }
    }

    private void SendHapticImpulse(InputDevice device, float amplitude, float duration)
    {
        if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }
}

