using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class pointToLineHaptics : hapticsPlayerParent
{
    [Header("Target Settings")]
    [SerializeField] List<Transform> targets;
    [SerializeField] Transform target;

    [SerializeField] Transform endOfLine;

    [Header("Controller Settings")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("Haptic Settings")]
    [SerializeField] float maxIntensity = 1.0f;
    [SerializeField] float minIntensity = 0.1f;
    //[SerializeField] float hapticDuration = 0.1f;
    [SerializeField] float intensityDiv = 10;
    [SerializeField] AnimationCurve hapticCurve;
    [SerializeField] AnimationCurve confirmCurve;

    [SerializeField] bool invertHaptics;


    protected float frequency = 440f; // Frequency in Hz
    protected float amplitude = 0.5f; // Amplitude (0.0 to 1.0)
    protected int sampleRate = 44100; // Sample rate (Hz)
    protected AudioSource audioSource;
    bool givingSound = false;

    Coroutine playingSound;

    bool onTarget = false;



    [SerializeField] protected hapticPattern confirmPatern;
    [SerializeField] protected hapticPattern awayPattern;
    

    [SerializeField] public controllerHandedness handedness = controllerHandedness.both;


    protected override void Start()
    {
        base.Start();
        audioSource = gameObject.AddComponent<AudioSource>();
    }



    /*void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, HandleUtility.DistancePointLine(target.position, transform.position, endOfLine.position) / intensityDiv);
        intensity = invertHaptics ? intensity : 1 - intensity;

        playPattern(intensity);
    }*/

    private void FixedUpdate()
    {
        float distance = 100;
        float remainingTime = 0;

        foreach (Transform t in targets)
        { 
            float dist = HandleUtility.DistancePointLine(t.position, transform.position, endOfLine.position) / intensityDiv;

            if (dist < distance)
            {
                distance = dist;
                remainingTime = t.GetComponent<Target>().remainingTime;
            }
        }

        distance = 1 - hapticCurve.Evaluate(distance);

        float intensity = Mathf.Lerp(minIntensity, maxIntensity, distance);

        ApplyHapticFeedback(intensity, remainingTime);
    }

    private void ApplyHapticFeedback(float intensity, float remainingTime)
    {
        if (intensity < 0)
        {
            intensity = 0;
        }
        if (intensity > 1)
        {
            intensity = 1;
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 1000) && hit.transform.GetComponent<Target>())
            {
                float normalizedTime = (remainingTime - 10) / 15;
                if (normalizedTime > 5)
                {
                    normalizedTime = 5;
                }
                if (normalizedTime < 0)
                {
                    normalizedTime = 0;
                }
                if (!givingSound)
                {
                    confirmPatern.manualPattern.pulses[0].delay = normalizedTime / 2;
                    confirmPatern.manualPattern.pulses[0].manualID.duration = 0.2f;

                    if (playingSound != null)
                    {
                        StopCoroutine(playingSound);
                    }
                    playingSound = StartCoroutine(playSound(confirmPatern.manualPattern, audioSource));
                }
                confirmPatern.manualPattern.pulses[0].delay = 0;
                confirmPatern.manualPattern.pulses[0].manualID.duration = normalizedTime;
                if (!givingFeedback)
                {
                    Debug.Log(normalizedTime);
                    playPattern(confirmCurve, confirmPatern, handedness);
                }
                onTarget = true;
                return;
            }
        }

        if (onTarget)
        {
            onTarget = false;
            playPattern(handedness, awayPattern);
        }
        else
        {
            float normalizedTime = (remainingTime - 10) / 15;

            if (normalizedTime > 1)
            {
                normalizedTime = 1;
            }
            if (normalizedTime < 0)
            {
                normalizedTime = 0;
            }
            hapticPattern.manualPattern.pulses[0].delay = normalizedTime;
            if (!givingFeedback)
            {
                playPattern(handedness, hapticPattern, intensity);
                if(playingSound != null)
                {
                    StopCoroutine(playingSound);
                    givingSound = false;
                    audioSource.Stop();
                }
                playingSound = StartCoroutine(playSound(confirmPatern.manualPattern, audioSource, intensity));
            }
        }
    }

    IEnumerator playSound(manualHapticPattern pattern, AudioSource audioSource, float amplitude = 1)
    {
        givingSound = true;
        audioSource.enabled = true;
        foreach (hapticPulse pulse in pattern.pulses)
        {
            float durationFix = pulse.manualID.duration;
            float delayFix = pulse.delay;

            AudioClip clip = createAudioClip(frequency, amplitude, sampleRate, pulse.manualID.duration);
            audioSource.clip = clip;
            audioSource.Play();
            
            yield return new WaitForSeconds(durationFix);

            audioSource.Stop();

            yield return new WaitForSeconds(delayFix);
        }
        givingSound = false;
    }

    IEnumerator playSound(AnimationCurve curve, manualHapticPattern pattern, AudioSource audioSource, float amplitude = 1)
    {
        givingSound = true;
        audioSource.enabled = true;
        foreach (hapticPulse pulse in pattern.pulses)
        {
            float durationFix = pulse.manualID.duration;
            float delayFix = pulse.delay;
            float time = 0;

            AudioClip clip = createAudioClip(frequency, amplitude, sampleRate, pulse.manualID.duration * 2);
            audioSource.clip = clip;
            audioSource.Play();


            while (time < durationFix)
            {
                float intensity = pulse.manualID.intensityL * curve.Evaluate(time / durationFix);
                intensity = Mathf.Clamp(intensity, 0, 1);

                audioSource.volume = intensity;

                time += Time.deltaTime;
                yield return null;
            }
            audioSource.Stop();

            yield return new WaitForSeconds(delayFix);
        }
        givingSound = false;
    }

    //i got this from chatgpt:
    AudioClip createAudioClip(float freq, float amp, int sampleRate, float duration)
    {
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = amp * Mathf.Sin(2 * Mathf.PI * freq * i / sampleRate);
        }

        AudioClip clip = AudioClip.Create("proceduralSound", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        return clip;
    }
}
