using JetBrains.Annotations;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float lastCutTime;
    public float timerDuration = 3f;

    public GameObject comboScreen;
    public TextMeshProUGUI comboText;

    private int comboScore;
    public static GameManager instance;

    private Vector3 originalComboScreenScale;

    public bool gameSelected;
    public GameObject gameSelectionScreen;

    private int fails = 0;

    public GameObject[] gameModes;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the last cut time to the current time
        lastCutTime = Time.time;
        comboScreen.SetActive(false);

        originalComboScreenScale = comboScreen.transform.localScale;

        comboScore = 0;

        gameSelected = false;
    }

    // Call this method when the player cuts a fruit
    public void PlayerCutFruit(Transform fruitPosition)
    {
        // Update the last cut time to the current time
        lastCutTime = Time.time;

        // Check if the comboScreenPosition is not null and the target is not destroyed
        comboScreen.transform.position = fruitPosition.position;
        comboScore++;

        comboScreen.transform.localScale = comboScreen.transform.localScale;
    }

    public void AddFailures()
    {
        //fails += 1;

        //if(fails >= 5 )
        //{
        //    gameSelected = false;

        //    for(int i = 0; i < gameModes.Length; i++) 
        //    {
        //        gameModes[i].gameObject.SetActive(false);
        //    }
        //}
    }

    private void Update()
    {
        // Check if the time since the last cut is greater than the timer duration
        if (Time.time - lastCutTime >= timerDuration)
        {
            // Timer has passed, reset combo score and deactivate the combo screen
            if (comboScreen.activeSelf)
            {
                comboScreen.transform.localScale = originalComboScreenScale;
                comboScreen.SetActive(false);
            }
            comboScore = 0;
        }

        if (comboScore >= 2)
        {
            comboScreen.SetActive(true);

            comboText.text = "Combo " + comboScore.ToString() + "x";
        }

        if (!gameSelected)
        {
            gameSelectionScreen.gameObject.SetActive(true);
        }
        else
        {
            gameSelectionScreen.gameObject.SetActive(false);
        }
    }
}