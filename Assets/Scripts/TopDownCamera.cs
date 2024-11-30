using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 startingOffset;

    void Awake()
    {
        startingOffset = transform.position;
    }
    
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + startingOffset.x, transform.position.y, target.transform.position.z + startingOffset.z);
    }
}
