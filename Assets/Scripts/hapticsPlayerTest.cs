using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class hapticsPlayerTest : hapticsPlayerParent
{

    [SerializeField] AnimationCurve clip;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //playPattern();
        StartCoroutine(playManualHaptics(hapticPattern.manualPattern, clip, 1));
    }

    private void Update()
    {
        if (!givingFeedback)
        {
            StartCoroutine(playManualHaptics(hapticPattern.manualPattern, clip, 1));
        }
    }


    protected IEnumerator playManualHaptics(manualHapticPattern pattern, AnimationCurve animationClip, float intensityMultiplier = 1f)
    {
        givingFeedback = true;
        foreach (hapticPulse pulse in pattern.pulses)
        {
            var rController = HapticsUtility.Controller.Right;
            var LController = HapticsUtility.Controller.Left;
            float time = 0;

            while(time < pulse.manualID.duration)
            {

                float intensityL = pulse.manualID.intensityL * animationClip.Evaluate(time / pulse.manualID.duration);
                float intensityR = pulse.manualID.intensityR * animationClip.Evaluate(time / pulse.manualID.duration);

                Mathf.Clamp(intensityL, 0, 1);
                Mathf.Clamp(intensityR, 0, 1);

                
                HapticsUtility.SendHapticImpulse(intensityR * intensityMultiplier, pulse.manualID.duration, rController, pulse.manualID.frequencyR);
                HapticsUtility.SendHapticImpulse(intensityL * intensityMultiplier, pulse.manualID.duration, LController, pulse.manualID.frequencyL);

                time += Time.deltaTime;
                yield return null;
            }
            Debug.Log("shitzooi" + time);
            yield return new WaitForSeconds(pulse.delay);
        }
        givingFeedback = false;
    }
}
