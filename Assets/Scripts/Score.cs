using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    Text scoreDisplay;
    bool victory;

    [SerializeField] string text;
    [SerializeField] int currentScore;
    public int maxScore = 30;

    public int CurrentScore { get => currentScore;
        set
        {
            if (value > maxScore)
            {
                value = maxScore;
            }

            if (value <= 0)
            {                
                if (!victory)
                {
                    victory = true;
                    EventManager.current.GameWin();
                    value = 0;
                }
            }

            currentScore = value;
        }
    }

    private void Awake()
    {
        scoreDisplay = GetComponent<Text>();
        CurrentScore = maxScore;
    }

    private void Start()
    {
        EventManager.onWinScore += AddScore;
    }

    public void AddScore()
    {
        CurrentScore--;
    }

    private void Update()
    {
        scoreDisplay.text = this.text + CurrentScore + "/" + maxScore;
    }
}
