using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class audioTests : MonoBehaviour
{
    public float frequency = 440f; // Frequency in Hz
    public float amplitude = 0.5f; // Amplitude (0.0 to 1.0)
    public int sampleRate = 44100; // Sample rate (Hz)
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip clip = createAudioClip(frequency, amplitude, sampleRate, 100.0f);
        audioSource.clip = clip;
        audioSource.Play();
        //int i = 0;


        for (int i = 0; i < 9; i++)
        {
            frequency *= 2;

            audioSource = gameObject.AddComponent<AudioSource>();
            clip = createAudioClip(frequency, amplitude, sampleRate, 100.0f);
            audioSource.clip = clip;
            audioSource.Play();
        }
        
    }

    AudioClip createAudioClip(float freq, float amp, int sampleRate, float duration)
    {
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            // Generate sine wave sample
            samples[i] = amp * Mathf.Sin(2 * Mathf.PI * freq * i / sampleRate);
        }

        AudioClip clip = AudioClip.Create("ProceduralSound", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        return clip;
    }
}
