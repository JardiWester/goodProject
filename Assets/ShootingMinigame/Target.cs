using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isTargetPractice;
    public float healt = 10f;
    public float defaultHealt;

    public float moveSpeed = 5f; 
    public bool moveForward = true; 

    private void Start()
    {
        defaultHealt = healt;

        gameObject.transform.position = new Vector3(Random.Range(10, -15), Random.Range(5, -5), Random.Range(30, 40));
        transform.rotation = Random.rotation;
    }

    private void Update()
    {
        float direction = moveForward ? 1f : -1f; // Forward (1) or backward (-1)
        transform.position += new Vector3(0, 0, direction) * moveSpeed * Time.deltaTime;
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
        if (isTargetPractice)
        {
            healt = defaultHealt;

            
            Respawn();
        }
        else
        {
            Destroy(gameObject); 
        }

        Debug.Log("Target dead");
    }

    void Respawn()
    {
        Vector3 newPosition = new Vector3(Random.Range(10, -15), Random.Range(5, -5), Random.Range(50, 90));
        
        transform.position = newPosition;

        transform.rotation = Random.rotation;

        moveForward = Random.value > 0.5f; 
    }
}