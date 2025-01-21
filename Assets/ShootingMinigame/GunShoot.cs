using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : hapticsPlayerParent
{
    public float damage = 10f;
    public float impactForce = 100f;
    public float range = 1000f;
    public float firingRate = 15f;

    private float nextTimeToFire = 0f;

    public controllerHandedness handedness;

    [SerializeField] AnimationCurve curve;

    [SerializeField] private InputActionReference lShootAction;
    [SerializeField] private InputActionReference rShootAction;
    private bool lShootActionPressed;
    private bool rShootActionPressed;



    void Awake()
    {
        lShootAction.action.performed += i => lShootActionPressed = true;
        rShootAction.action.performed += i => rShootActionPressed = true;
    }



    private void Update()
    {
        if (lShootActionPressed)
        {
            //Debug.Log("gughguhgughughguh");
            lShootActionPressed = false;
            //nextTimeToFire = Time.time + 1f / firingRate;
            if ((handedness == controllerHandedness.left || handedness == controllerHandedness.both))
            {
                Shoot();
            }
        }
        if (rShootActionPressed)
        {
            //Debug.Log("gughguhgughughguh");
            rShootActionPressed = false;
            //nextTimeToFire = Time.time + 1f / firingRate;
            if ((handedness == controllerHandedness.right || handedness == controllerHandedness.both))
            {
                Shoot();
            }
        }

   }


   void Shoot()
   {
      if (Physics.Raycast(transform.position, transform.forward, out var hit, range))
      {
         //Debug.Log(hit.transform.name);
         
         if( hit.rigidbody != null)
         {
            hit.rigidbody.AddForce(-hit.transform.forward * impactForce);
         }
         Target target = hit.transform.GetComponent<Target>();
         if (target != null)
         {
            target.TakeDamage(damage);
                playPattern(curve, hapticPattern, handedness);
         }

      }
      
   }
}
