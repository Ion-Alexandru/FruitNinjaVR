using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public string gameMode;
    public GameObject buttonFruitPrefab;

    private GameObject buttonFruit;

    public GameObject fruitSpawner;
    public GameObject verticalFruitSpawner;

    private bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newFruitButton =  Instantiate(buttonFruitPrefab, transform.position, Quaternion.identity);
        buttonFruit = newFruitButton;
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

        if(buttonFruit == null && !gameStarted)
        {
            if(gameMode.Contains("Classic GameMode"))
            {
                fruitSpawner.gameObject.SetActive(true);
            } else if (gameMode.Contains("Vertical GameMode"))
            {
                verticalFruitSpawner.gameObject.SetActive(true);
            }

            gameStarted = true;
            GameManager.instance.gameSelected = gameStarted;
        }
    }
}
