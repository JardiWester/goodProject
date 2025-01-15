using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticSlider : hapticsPlayerParent
{
    [SerializeField] protected hapticPattern stopHapticPattern;

    [SerializeField] private bool changed = false;


    public void valueChanged(Single newValue)
    {
        playPattern(null, newValue);
        changed = true;
    }

    private void Update()
    {
        if (changed && !givingFeedback)
        {
            changed = false;
            playPattern(stopHapticPattern);
        }
    }
}
