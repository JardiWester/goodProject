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

public enum controllerHandedness
{
    left,
    right,
    both
}


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
    public float delay;

    //public HapticClip pattern { get => Pattern; }
    public manualIFD manualID { get => ManualID; }
}
[Serializable]
public class manualIFD
{
    

    [SerializeField] public float IntensityL;
    [SerializeField] private float FrequencyL;
    [Space(10)]
    [SerializeField] public float IntensityR;
    [SerializeField] private float FrequencyR;
    [Space(15)]
    [SerializeField] public float duration;
    

    public float intensityL { get => IntensityL; }
    public float frequencyL { get => FrequencyL; }
    public float intensityR { get => IntensityR; }
    public float frequencyR { get => FrequencyR; }
    //public float duration { get => Duration; }
}

public class hapticsPlayerParent : MonoBehaviour
{
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;
    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
    [SerializeField] protected hapticPattern hapticPattern;
    [SerializeField] protected bool givingFeedback;


    private Coroutine playHaptics;
    

    protected virtual void Start()
    {
        //UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, device);

        //debug stuff:
        //OVRInput.SetControllerVibration(200, 1);
        //playPattern();
        /*
        var rController = HapticsUtility.Controller.Right;
        HapticsUtility.SendHapticImpulse(1, 10, rController, 1);
        */
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
    
    public void playPattern()
    {
        playPattern(hapticPattern, 1);
    }

    public void playPattern(controllerHandedness handedness, hapticPattern playHapticPattern = null, float intensityMultiplier = 1)
    {
        if (playHapticPattern == null)
        {
            playHapticPattern = hapticPattern;
        }



        if (handedness == controllerHandedness.left)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 1;
                pulse.manualID.IntensityR = 0;
            }

        } else if (handedness == controllerHandedness.right)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 0;
                pulse.manualID.IntensityR = 1;
            }
        }
        else if (handedness == controllerHandedness.both)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 1;
                pulse.manualID.IntensityR = 1;
            }
        }

        playPattern(playHapticPattern, intensityMultiplier);
    }

    public void playPattern(hapticPattern playHapticPattern)
    {
        //Debug.Log(playHapticPattern);
        playPattern(playHapticPattern, 1);
    }
    public void playPattern(float intensityMultiplier)
    { 
        playPattern(hapticPattern, intensityMultiplier);
    }

    public void playPattern(hapticPattern playHapticPattern, float intensityMultiplier)
    {
        /*
        if (Gamepad.current == null)
        {
            return;
        }
        */

        
        //StopCoroutine(playManualHaptics(null));

        //playPattern(playHapticPattern);
        //playPattern(intensityMultiplier);

        if (playHaptics != null)
        {
            //Debug.Log("shitzooi");

            StopCoroutine(playHaptics);
        }
        
        playHaptics = StartCoroutine(playManualHaptics(playHapticPattern.manualPattern, intensityMultiplier));
    }

    public void playPattern(AnimationCurve curve, hapticPattern playHapticPattern, controllerHandedness handedness = controllerHandedness.both, float intensityMultiplier = 1)
    {
        if (handedness == controllerHandedness.left)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 1;
                pulse.manualID.IntensityR = 0;
            }

        }
        else if (handedness == controllerHandedness.right)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 0;
                pulse.manualID.IntensityR = 1;
            }
        }
        else if (handedness == controllerHandedness.both)
        {
            foreach (hapticPulse pulse in playHapticPattern.manualPattern.pulses)
            {
                pulse.manualID.IntensityL = 1;
                pulse.manualID.IntensityR = 1;
            }
        }

        if (playHaptics != null)
        {
            //Debug.Log("kakazooi");

            StopCoroutine(playHaptics);
        }



        StartCoroutine(playManualHaptics(playHapticPattern.manualPattern, curve, intensityMultiplier));
    }

    protected virtual IEnumerator playManualHaptics(manualHapticPattern pattern, float intensityMultiplier = 1f)
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

    protected IEnumerator playManualHaptics(manualHapticPattern pattern, AnimationCurve animationClip, float intensityMultiplier = 1f)
    {
        manualHapticPattern patternFix = pattern;
        givingFeedback = true;
        foreach (hapticPulse pulse in patternFix.pulses)
        {
            var rController = HapticsUtility.Controller.Right;
            var LController = HapticsUtility.Controller.Left;
            float time = 0;



            float durationFix = pulse.manualID.duration;
            float delayFix = pulse.delay;


            //Debug.Log(pulse.manualID.duration);

            while (time < durationFix)
            {

                float intensityL = pulse.manualID.intensityL * animationClip.Evaluate(time / durationFix);
                float intensityR = pulse.manualID.intensityR * animationClip.Evaluate(time / durationFix);

                //Debug.Log(time / pulse.manualID.duration + " " + time / pulse.manualID.duration);
                //Debug.Log(time + " " + pulse.manualID.duration);

                

                intensityL = Mathf.Clamp(intensityL, 0, 1);
                intensityR = Mathf.Clamp(intensityR, 0, 1);

                //Debug.Log(intensityR * intensityMultiplier);
                //Debug.Log(intensityR);

                //Debug.Log(time + " " + pulse.manualID.duration);
                //Debug.Log(intensityL + " " + intensityR); 
                HapticsUtility.SendHapticImpulse(intensityR * intensityMultiplier, 0, rController, pulse.manualID.frequencyR);
                HapticsUtility.SendHapticImpulse(intensityL * intensityMultiplier, 0, LController, pulse.manualID.frequencyL);

                time += Time.deltaTime;
                yield return null;
            }
            //Debug.Log(patternFix.pulses[0].manualID.duration);
            //Debug.Log("shitzooi   " + time + " " + pulse.manualID.duration);
            //Debug.Log(delayFix);
            yield return new WaitForSeconds(delayFix);
        }
        givingFeedback = false;
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
