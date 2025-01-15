using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;
//using SpeachLib;

public class textToSpeechTests : MonoBehaviour
{
    [SerializeField] string textToSpeek;

    //SpVoice voice = new SpVoice();


    // Start is called before the first frame update
    void Start()
    {
        //voice.Speak(textToSpeek, SpeechVoiceFlags.SVSGlagsAsync | SpeachVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
