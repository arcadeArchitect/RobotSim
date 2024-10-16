using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WheelBehavior : MonoBehaviour
{
    private WheelCollider _wc;
    private Transform _visualWheel;

    void Start()
    {
        if (this.transform.childCount == 0) {
            Debug.Log("need child wheel");  
            return;
        }
        _wc = gameObject.GetComponent<WheelCollider>();
        _visualWheel = this.transform.GetChild(0);
    }

    public void FixedUpdate() 
    {

        _wc.steerAngle = this.transform.localEulerAngles.y;

        if (_wc) {
            _wc.GetWorldPose(out var position, out var rotation);
            _visualWheel.transform.position = position;
            _visualWheel.transform.rotation = rotation;
            _wc.rotationSpeed = 360;
        }
    }
}
