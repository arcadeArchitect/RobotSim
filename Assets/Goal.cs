using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private int score = 1;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (!projectile) projectile = other.GetComponentInParent<Projectile>();
        if (!projectile) return;
        
        Debug.Log("Adding score!");
        ScoreManager.Instance.AddScore(team, score);
    }
}
