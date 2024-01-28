using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreenScript : MonoBehaviour
{
    private GameManager gameManager;
    private int score;
    private TextMeshProUGUI scoreText;
    private bool newGame = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        score = 0;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.gameObject.SetActive(gameManager.gameSelected);
        
        scoreText.text = score.ToString();
    }

    public void addScore()
    {
        score += 1 * gameManager.comboScore;
    }
}
