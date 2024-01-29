using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour
{
    private GameManager gameManager;
    private int score;
    public TextMeshProUGUI scoreText;
    private bool newGame = false;

    public UnityEngine.UI.Image[] hearts;
    private int lives;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        score = 0;
        lives = hearts.Length;

        scoreText.text = "Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.gameObject.SetActive(gameManager.gameSelected);
        
        scoreText.text = "Score: " + score.ToString();
    }

    public void addScore()
    {
        score += 1 * gameManager.comboScore;
    }

    public void takeHearts()
    {
        if (lives > 0)
        {
            lives--;

            Destroy(hearts[lives].gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DestroyScoreScreen()
    {
        Destroy(gameObject);
    }
}
