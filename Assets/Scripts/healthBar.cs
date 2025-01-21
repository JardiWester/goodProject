using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    Vector3 maxSize;
    RectTransform rectTransform;


    public static healthBar instance { get; private set; }

    //i'm using awake so any other script can already acces the healthBar on start
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //optional: persist this instance between scenes
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        maxSize = rectTransform.localScale;
        //adjustHealth(0.2f, 0.4f);
    }

    public void adjustHealth(float health, float maxHealth = 1)
    {

        float newSize = maxSize.x * health/maxHealth;

        if (newSize < 0)
        {
            newSize = 0;
            Debug.Log("tried making the healthbar a negative size");
        }
        if (newSize > maxSize.x)
        {
            newSize = maxSize.x;
            Debug.LogWarning("tried making the healthbar too big");
        }

        rectTransform.localScale = new Vector3 (newSize, maxSize.y, maxSize.z);
    }
}
