using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Camera camera = Camera.main;

        if (gameObject.transform.position.y <= -1)
        {
            gameManager.AddFailures();
            Destroy(gameObject);
        } else if(gameObject.transform.position.z < camera.transform.position.z - 1)
        {
            gameManager.AddFailures();
            Destroy(gameObject);
        }
    }
}
