using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;

public class pointToLineHaptics : hapticsPlayerParent
{
    [Header("Target Settings")]
    [SerializeField] List<Transform> targets;

    [SerializeField] Transform endOfLine;

    [Header("Controller Settings")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("Haptic Settings")]
    public float maxIntensity = 1.0f;
    public float minIntensity = 0.1f;
    public float hapticDuration = 0.1f;
    [SerializeField] float intensityDiv = 10;
    [SerializeField] bool invertHaptics = false;

    [SerializeField] protected hapticPattern confirmPatern;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.Lerp(minIntensity, maxIntensity, HandleUtility.DistancePointLine(target.position, transform.position, endOfLine.position) / intensityDiv));

        float distance = 10000;

        foreach (Transform t in targets)
        {
            float dist = HandleUtility.DistancePointLine(t.position, transform.position, endOfLine.position) / intensityDiv;
            if (dist < distance)
            {
                distance = dist;
            }
        }




        float intensity = Mathf.Lerp(minIntensity, maxIntensity, distance);
        ApplyHapticFeedback(intensity);
        //Debug.Log(intensity);
    }

    private void ApplyHapticFeedback(float intensity)
    {
        hapticPattern paternToPlay = hapticPattern;

        if (invertHaptics)
        {
            intensity = 1 - intensity;
        }
        if (intensity < 0)
        {
            intensity = 0;
        }
        
        if (intensity > 1)
        {
            intensity = 1;
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 1000) && hit.transform.GetComponent<Target>())
            {
                //Debug.Log("hit");
                if (!givingFeedback)
                {
                    playPattern(confirmPatern);
                }
            } else
            {
                //if (!givingFeedback)
                //{
                    //hapticPattern.manualPattern.pulses[0].manualID.duration = 1 - intensity;
                    playPattern(hapticPattern, intensity);
                //}
            }
        }
        else
        {
            //if (!givingFeedback)
            //{
                //hapticPattern.manualPattern.pulses[0].manualID.duration = 1 - intensity;
                playPattern(hapticPattern, intensity);
            //}
        }



        

        //Debug.Log(intensity);
        InputDevice hapticDevice = InputDevices.GetDeviceAtXRNode(controllerNode);


        if (hapticDevice.isValid)
        {
            //SendHapticImpulse(hapticDevice, intensity, hapticDuration);
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
