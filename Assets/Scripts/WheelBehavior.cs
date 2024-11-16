using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WheelBehavior : MonoBehaviour
{
    private WheelCollider wc;
    private Transform visualWheel;

    void Start()
    {
        if (transform.childCount == 0) {
            Debug.Log("need child wheel");  
            return;
        }
        wc = gameObject.GetComponent<WheelCollider>();
        visualWheel = transform.GetChild(0);
    }

    public void FixedUpdate() 
    {
        wc.steerAngle = this.transform.localEulerAngles.y;

        if (wc) {
            wc.GetWorldPose(out var position, out var rotation);
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }
    }

    public void SetTorque(float torque)
    {
        wc.motorTorque = torque;
    }

    public void SetBrake(float force)
    {
        wc.brakeTorque = force;
    }
}
