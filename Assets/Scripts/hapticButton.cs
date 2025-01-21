using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticButton : hapticsPlayerParent
{
    [SerializeField] hapticPattern hoverpattern;
    public void buttonPressed()
    {
        playPattern();
    }

    public void hover()
    {
        playPattern(hoverpattern);
    }

}
