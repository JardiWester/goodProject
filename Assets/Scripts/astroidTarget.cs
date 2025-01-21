using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class astroidTarget : MonoBehaviour
{
    public static Transform target;
    public static Vector2 distanceRange;
    public static Vector2 speedRange;

    [SerializeField] Vector2 DistanceRange;
    [SerializeField] Vector2 SpeedRange;
    [SerializeField] UnityEvent astroidHit;
    [SerializeField] UnityEvent astroidDestroy;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;
    [SerializeField] float health;



    private void Awake()
    {
        target = transform;
        distanceRange = DistanceRange;
        speedRange = SpeedRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*float maxSpeed = 0;
        float minSpeed = Mathf.Infinity;
        foreach (Target astroid in Target.targets)
        {
            if(maxSpeed < astroid.SpeedMult)
            {
                maxSpeed = astroid.SpeedMult;
            }
            if (minSpeed > astroid.SpeedMult)
            {
                minSpeed = astroid.SpeedMult;
            }
        }
        foreach (Target astroid in Target.targets)
        {
            if (minSpeed != maxSpeed)
            {
                astroid.speedMultNormalized = (astroid.SpeedMult - minSpeed) / (maxSpeed - minSpeed);
            }
            else
            {
                astroid.speedMultNormalized = 0.5f;
            }
        }*/

        health = maxHealth;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("astroid"))
        {
            astroidHit.Invoke();
            if (other.GetComponent<Target>())
            {
                other.GetComponent<Target>().Respawn();

            }
            else
            {
                other.gameObject.SetActive(false);
            }
            health -= damage;
            healthBar.instance.adjustHealth(health, maxHealth);

            if(health <= 0)
            {
                destroy();
            }
        }
    }


    private void destroy()
    {
        astroidDestroy.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //target = transform;
        distanceRange = DistanceRange;
        speedRange = SpeedRange;
#endif


    }

    
}
