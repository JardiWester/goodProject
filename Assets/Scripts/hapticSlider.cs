using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticSlider : hapticsPlayerParent
{
    [SerializeField] hapticPattern stopHapticPattern;
    [SerializeField] hapticPattern hoverpattern;

    [SerializeField] private bool changed = false;

    public void hover()
    {
        playPattern(hoverpattern);
    }

    public void valueChanged(Single newValue)
    {
        playPattern(newValue);
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
