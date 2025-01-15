using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class hapticsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnApplicationQuit()
    {
        //Debug.Log("jeej");
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
