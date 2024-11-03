using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue,
    Red
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private int blueScore;
    [SerializeField] private int redScore;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); 
    }

    public void AddScore(Team team, int score)
    {
        if (team == Team.Blue)
            blueScore += score;
        else
            redScore += score;
    }
}
