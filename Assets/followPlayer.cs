using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        if (!playerTransform)
        {
            playerTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = playerTransform.position;
    }
}
