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
    [SerializeField] private TMP_Text blueScoreCounter;
    [SerializeField] private TMP_Text redScoreCounter;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        blueScoreCounter.text = "0";
        redScoreCounter.text = "0";
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
        blueScoreCounter.text = blueScore.ToString();
        redScoreCounter.text = redScore.ToString();
    }
            
}
