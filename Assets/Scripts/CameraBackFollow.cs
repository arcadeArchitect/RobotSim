
using UnityEngine;
using UnityEngine.Serialization;

public class CameraBackFollow : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private bool lockY = false;
    private float xRotation;
    private Vector3 offset;
    private void Start()
    {
        xRotation = transform.eulerAngles.x;
        offset = transform.position - follow.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredRotation = new Vector3(xRotation, follow.eulerAngles.y, 0);
        transform.localEulerAngles = desiredRotation;
        
        float rotationRads = transform.localEulerAngles.y * Mathf.Deg2Rad;
        Vector3 desiredPosition = follow.position + new Vector3(offset.z * Mathf.Sin(rotationRads), lockY ? offset.y - follow.position.y : offset.y, offset.z * Mathf.Cos(rotationRads));
        transform.position = desiredPosition;
        
        // Vector3 desiredPosition = follow.position + follow.TransformDirection(offset.z * Vector3.forward);
        // desiredPosition.y += offset.y;
        // transform.position = desiredPosition;
    }
}
