using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public string gameMode;
    public GameObject buttonFruitPrefab;

    private GameObject buttonFruit;

    public GameObject fruitSpawner;
    public GameObject verticalFruitSpawner;

    public bool gameStarted = false;

    // Score Screen logic
    public Transform scoreScreenPosition;
    public GameObject scoreScreen;

    // Start is called before the first frame update
    void Start()
    {
        ReplaceFruitButtons();
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }

    public void ReplaceFruitButtons()
    {
        GameObject newFruitButton =  Instantiate(buttonFruitPrefab, transform.position, Quaternion.identity);
        buttonFruit = newFruitButton;
        buttonFruit.transform.SetParent(transform);
    }
}
