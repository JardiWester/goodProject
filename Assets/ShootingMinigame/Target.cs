using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    public bool isTargetPractice;
    public float healt = 10f;
    public float defaultHealt;

    private void Start()
    {
        defaultHealt = healt;
        gameObject.transform.position = new Vector3(Random.Range(370,415), Random.Range(380,385), Random.Range(1,20));
        transform.rotation = Random.rotation;
    }

    public void TakeDamage(float amount)
    {
        healt -= amount;
        if (healt <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isTargetPractice == true)
        {
            healt = defaultHealt;
            gameObject.transform.position = new Vector3(Random.Range(370,415), Random.Range(380,385), Random.Range(1,20));
            gameObject.transform.rotation = Random.rotation;
        }
        else
        {
            Destroy(gameObject);
        }
        //Debug.Log("target dead");
    }
}
