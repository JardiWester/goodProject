using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class hapticsPlayerTest : hapticsPlayerParent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playPattern();
    }
    
}
