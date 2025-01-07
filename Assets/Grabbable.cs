using UnityEngine;
using UnityEngine.Serialization;

public class Grabbable : MonoBehaviour
{
    public ArticulationBody attachBody;
    public Transform robotChassis;
    public bool isBeingGrabbed;
       

    public void MoveBody(Vector3 movement)
    {
        robotChassis.Translate(movement, Space.World);
    }

    public void Update()
    {
        if (isBeingGrabbed) MoveBody(transform.up * (Time.deltaTime * 0.1f));
    }
}
