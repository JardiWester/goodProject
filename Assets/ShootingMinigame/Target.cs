using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.XR;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Target : MonoBehaviour
{
    public bool isTargetPractice;
    public float health = 10f;
    //public bool moveForward = true;

    [SerializeField]
    [Range(0f, 1f)]
    float speedMult = 1;

    public float speedMultNormalized;

    public float remainingTime;

    public float SpeedMult { get => speedMult; }


    public static List<Target> targets = new List<Target>();


    Transform target;

    float moveSpeed = 5f;
    float defaultHealth;

    private void Awake()
    {
        targets.Add(this);
    }

    void Start()
    { 
        target = astroidTarget.target;
        if (!target || astroidTarget.distanceRange == Vector2.zero || astroidTarget.speedRange == Vector2.zero)
        {
            Debug.LogError("astroidTarget has unassigned variables");
        }

        defaultHealth = health;
        Respawn();
    }

    void Update()
    {
        /*
        float direction = moveForward ? 1f : -1f; // Forward (1) or backward (-1)
        transform.position += transform.forward * -direction * moveSpeed * Time.deltaTime;//   new Vector3(0, 0, direction) * moveSpeed * Time.deltaTime;
        */
        //target = astroidTarget.target;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * speedMult * Time.deltaTime);

        remainingTime = Vector3.Distance(transform.position, target.position) / moveSpeed;


    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isTargetPractice)
        {
            Respawn();
        }
        else
        {
            targets.Remove(this);
            Destroy(gameObject);
        }

        //Debug.Log("Target dead");
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("jipee");
        if (other.CompareTag("astroidTarget"))
        {
            //Respawn();
        }
    }

    public void Respawn()
    {
        health = defaultHealth;
        
        /*
        Vector3 newPosition = new Vector3(Random.Range(10, -15), Random.Range(5, -5), Random.Range(50, 90));

        transform.position = newPosition;
        */

        //transform.rotation = Quaternion.Euler( new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 0)));  //Random.rotation;

        transform.position = target.position;

        transform.LookAt(transform.position + new Vector3(Random.Range(-90, 90), Random.Range(0, 90), Random.Range(0, 90)));


        transform.position += transform.forward * Random.Range(astroidTarget.distanceRange.x, astroidTarget.distanceRange.y);

        moveSpeed = Random.Range(astroidTarget.speedRange.x, astroidTarget.speedRange.y);

        //moveForward = Random.value > 0.5f;
    }
}