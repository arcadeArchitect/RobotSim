using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    public float angle;

    void Update()
    {
        angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
    }
    
    public float GetRobotAngle()
    {
        return angle * Mathf.Deg2Rad;
    }
}
