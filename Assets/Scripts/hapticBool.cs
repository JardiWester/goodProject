using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticBool : hapticsPlayerParent
{
    [SerializeField] protected hapticPattern falseHapticPattern;

    public void boolChanged(bool newValue)
    {
        if (newValue)
        {
            playPattern();
        }else
        {
            playPattern(falseHapticPattern);
        }
    }
}
