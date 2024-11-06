using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum Team
{
    Blue,
    Red
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private int blueScore;
    [SerializeField] private int redScore;
    [SerializeField] private TMP_Text scoreCounter;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        scoreCounter.text = "Score: 0";
    }

    public void AddScore(Team team, int score)
    {
        if (team == Team.Blue)
            blueScore += score;
        else
            redScore += score;
        
        UpdateScoreCounter();
    }
    
    private void UpdateScoreCounter()
    {
        scoreCounter.text = "Score: " + blueScore;
    }
            
}
