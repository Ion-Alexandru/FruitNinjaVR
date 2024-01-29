using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public GameObject fruitSplashParticle;
    private GameManager gameManager;
    private ScoreScreenScript scoreScreenScript;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreScreenScript = FindObjectOfType<ScoreScreenScript>();
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

    public void FruitSliced()
    {
        Instantiate(fruitSplashParticle, transform.position, Quaternion.identity);

        if(scoreScreenScript != null)
        {
            scoreScreenScript.addScore();
        }

        //sliceSound.Play();
    }
}
