using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Projectile projectile;
    [SerializeField] private Team team;

    private void OnTriggerEnter(Collider other)
    {
        projectile = other.GetComponent<Projectile>();
        if (!projectile) projectile = other.GetComponentInParent<Projectile>();
        if (!projectile) return;
        
        Debug.Log("Adding score!");
        ScoreManager.Instance.AddScore(team, 1);
    }
}
