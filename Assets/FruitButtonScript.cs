using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        ButtonScript buttonScript = transform.parent.GetComponent<ButtonScript>();

        if(!gameManager.gameSelected)
        {
            if(buttonScript.gameMode.Contains("Classic GameMode"))
            {
                buttonScript.fruitSpawner.gameObject.SetActive(true);
                Instantiate(buttonScript.scoreScreen, buttonScript.scoreScreenPosition.position, Quaternion.identity);
            } else if (buttonScript.gameMode.Contains("Vertical GameMode"))
            {
                buttonScript.verticalFruitSpawner.gameObject.SetActive(true);
                Instantiate(buttonScript.scoreScreen, buttonScript.scoreScreenPosition.position, Quaternion.identity);
            }

            gameManager.gameSelected = true;

            DestroyFruitButtons();
        }
    }

    void DestroyFruitButtons()
    {
        // Find all GameObjects with the "fruit" layer
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("FruitButton");

        // Destroy each fruit found
        foreach (GameObject fruit in fruits)
        {
            Destroy(fruit);
        }
    }
}
