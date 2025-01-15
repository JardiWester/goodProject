using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
   public float damage = 10f;
   public float impactForce = 100f;
   public float range = 1000f;
   public float firingRate = 15f;

   public Transform fpsCam;
   private float nextTimeToFire = 0f;

    [SerializeField] private InputActionReference shootAction;
    private bool shootActionPressed;



    void Awake()
    {
        shootAction.action.performed += i => shootActionPressed = true;
    }



    private void Start()
    {
        fpsCam = transform;
    }

    private void Update()
   {
        if (shootActionPressed)
        {
            //Debug.Log("gughguhgughughguh");
            shootActionPressed = false;
            //nextTimeToFire = Time.time + 1f / firingRate;
            Shoot();
        }

   }


   void Shoot()
   {
      if (Physics.Raycast(fpsCam.position, fpsCam.forward, out var hit, range))
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
         }
      }
      
   }
}
