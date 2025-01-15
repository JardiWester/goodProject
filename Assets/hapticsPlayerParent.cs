using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.InputSystem;
//using Oculus.Haptics;
using System;
//using static UnityEngine.Rendering.DynamicArray<T>;

//[RequireComponent(typeof(HapticClipPlayer))]



[Serializable]
public class hapticPattern
{
    //[SerializeField] private HapticClip Pattern = null;
    [SerializeField] private manualHapticPattern ManualPattern;

    //public HapticClip pattern { get => Pattern; }
    public manualHapticPattern manualPattern { get => ManualPattern; }
    //[SerializeField] private List<hapticPulse> Pulses;
}
[Serializable]
public class manualHapticPattern
{
    [SerializeField] private List<hapticPulse> Pulses;

    public List<hapticPulse> pulses { get => Pulses; }
}
[Serializable]
public class hapticPulse
{
    //[SerializeField] private HapticClip Pattern;
    [SerializeField] private manualIFD ManualID;
    [SerializeField] private float Delay;

    //public HapticClip pattern { get => Pattern; }
    public manualIFD manualID { get => ManualID; }
    public float delay { get => Delay; }
}
[Serializable]
public class manualIFD
{
    [SerializeField] private float IntensityL;
    [SerializeField] private float FrequencyL;
    [Space(10)]
    [SerializeField] private float IntensityR;
    [SerializeField] private float FrequencyR;
    [Space(15)]
    [SerializeField] private float Duration;

    

    public float intensityL { get => IntensityL; }
    public float frequencyL { get => FrequencyL; }
    public float intensityR { get => IntensityR; }
    public float frequencyR { get => FrequencyR; }
    public float duration { get => Duration; }
}

public class hapticsPlayerParent : MonoBehaviour
{
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;
    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
    [SerializeField] protected hapticPattern hapticPattern;
    [SerializeField] protected bool givingFeedback;

    protected virtual void Start()
    {
        var rController = HapticsUtility.Controller.Right;

        HapticsUtility.SendHapticImpulse(1, 10, rController, 1);

        //UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, device);

        //debug stuff:
        //OVRInput.SetControllerVibration(200, 1);
        //playPattern();
    }

    void Update()
    {
        

        //debug stuff:
        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("bruh");
            playPattern();
        }*/
    }
    

    public void playPattern(hapticPattern playHapticPattern = null)
    {
        if (playHapticPattern == null)
        {
            playHapticPattern = hapticPattern;
        }
        else
        {
            //Debug.Log("test");
        }
        playPattern(hapticPattern, 1);
    }
    public void playPattern(float intensityMultiplier)
    { 
        playPattern(hapticPattern, intensityMultiplier);
    }

    public void playPattern(hapticPattern playHapticPattern, float intensityMultiplier)
    {
        if (Gamepad.current == null)
        {
            //return;
        }

        StopAllCoroutines();
        //StopCoroutine(playManualHaptics(null));

        //playPattern(playHapticPattern);
        //playPattern(intensityMultiplier);


        
        StartCoroutine(playManualHaptics(hapticPattern.manualPattern, intensityMultiplier));
    }


    IEnumerator playManualHaptics(manualHapticPattern pattern, float intensityMultiplier = 1f)
    {
        

        
        givingFeedback = true;
        foreach (hapticPulse pulse in pattern.pulses)
        {
            /*foreach (var device in devices)
            {
                UnityEngine.XR.HapticCapabilities capabilities;
                if (device.TryGetHapticCapabilities(out capabilities))
                {
                    if (capabilities.supportsImpulse)
                    {
                        uint channel = 0;
                        float amplitude = 0.5f;
                        float duration = 1.0f;
                        device.SendHapticImpulse(0, pulse.manualID.intensityL * intensityMultiplier, duration);
                    }
                }
            }*/

            var rController = HapticsUtility.Controller.Right;

            HapticsUtility.SendHapticImpulse(pulse.manualID.intensityR * intensityMultiplier, pulse.manualID.duration, rController, pulse.manualID.frequencyR);

            var LController = HapticsUtility.Controller.Left;

            HapticsUtility.SendHapticImpulse(pulse.manualID.intensityL * intensityMultiplier, pulse.manualID.duration, LController, pulse.manualID.frequencyL);


            //Gamepad.current.SetMotorSpeeds(pulse.manualID.intensityL * intensityMultiplier, pulse.manualID.intensityR * intensityMultiplier);

            Debug.Log("guuuuuuuuuh");

            //OVRInput.SetControllerVibration(pulse.manualID.frequencyL, pulse.manualID.intensityL, OVRInput.Controller.LTouch);
            //OVRInput.SetControllerVibration(pulse.manualID.frequencyR, pulse.manualID.intensityR, OVRInput.Controller.RTouch);

            yield return new WaitForSeconds(pulse.manualID.duration);
            //Gamepad.current.SetMotorSpeeds(0, 0);

            //OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            //OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

            yield return new WaitForSeconds(pulse.delay);
        }
        givingFeedback = false;

        //debug stuff:
        //playPattern();
        //playPattern(pattern);
        //playPattern(hapticPattern, 1f);
    }


    void OnApplicationQuit()
    {
        //Debug.Log("jeej");
        StopAllCoroutines();
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

        //OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        //OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }


}
