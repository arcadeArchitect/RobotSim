using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DriveSystem : Subsystem
{
    [SerializeField] private Module frontRightModule;
    [SerializeField] private Module frontLeftModule;
    [SerializeField] private Module backRightModule;
    [SerializeField] private Module backLeftModule;
    
    [SerializeField] private Gyro gyro;

    [Space] [SerializeField] private float maxTorque = 100f;
    [SerializeField] private float rotationSpeed = Mathf.PI / 2;

    // distance between front and rear wheels (wheelbase)
    [Space] [SerializeField] private float length;
    
    // distance between wheels on the same axel (trackwidth)
    [SerializeField] private float width;

    [Space] [SerializeField] private bool fieldCentric = true;

    [SerializeField] private Vector3 lastInput;
    private Vector2 translation;

    private float frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed;
    private float frontRightAngle, frontLeftAngle, backRightAngle, backLeftAngle;

    public bool isAutoing;
    private Node currentNode;
    public PathManager pathManager;
    public int nodeIndex;

    public float tVal;
    public float tStep = 0.05f;
    public float tJump = 20;
    [Range(0, 0.9f)] public float pathFollowPriority = 0.9f;

    private Vector3? currentNodePos;

    public Transform robotBody;

    private void Start()
    {
        if (isAutoing)
            currentNode = pathManager.path[0];
        
        length = frontRightModule.GetWheelOffset().z - backRightModule.GetWheelOffset().z;
        width = frontRightModule.GetWheelOffset().x - frontLeftModule.GetWheelOffset().x;
    }

    public void Update()
    {
        if (isAutoing)
        {
            currentNodePos = pathManager.PathPoint(tVal);
            for (int i = 0; i < tJump + tStep; i++)
            {
                Vector3? newNodePos = pathManager.PathPoint(tVal + tStep);
                
                if (currentNodePos == null || newNodePos == null)
                {
                    isAutoing = false;
                    lastInput = Vector3.zero;
                    pathManager.nodes[^1].Act();
                    return;
                }
                
                if (Vector3.Distance(transform.position, currentNodePos.Value) >
                    Vector3.Distance(transform.position, newNodePos.Value))
                    tVal += tStep;
            }
            
            Vector3 nodeVel = pathManager.PathVel(tVal);
            if (currentNodePos != null)
            {
                Vector3 distanceToNode = currentNodePos.Value - transform.position;
                nodeVel = (1 - pathFollowPriority) * nodeVel + pathFollowPriority * distanceToNode;
            }

            BezierNode closestBezier = pathManager.nodes[(int)tVal];
            if (Vector3.Distance(transform.position, closestBezier.position) < 0.5f)
                closestBezier.Act();
            
            nodeVel.Normalize();
            
            Vector3 driveInput = new Vector3(nodeVel.x, nodeVel.z, 0);
            lastInput = driveInput;
        }
        SwerveDrive(lastInput);
        frontRightModule.SetInputAndRotation(frontRightSpeed, frontRightAngle);
        frontLeftModule.SetInputAndRotation(frontLeftSpeed, frontLeftAngle);
        backRightModule.SetInputAndRotation(backRightSpeed, backRightAngle);
        backLeftModule.SetInputAndRotation(backLeftSpeed, backLeftAngle);
    }

    public void SwerveDrive(InputAction.CallbackContext context)
    {
        if (isAutoing) return;
        lastInput = context.ReadValue<Vector3>();
    }

    private void SwerveDrive(Vector3 input) 
    {
        ThirdAlgorithm(input);
    }

    public void ToggleFieldCentric()
    {
        fieldCentric = !fieldCentric;
    }

    private void ThirdAlgorithm(Vector3 input)
    {
        float angle = gyro.GetRobotAngle();
        translation = fieldCentric ? new Vector2(-input.y * Mathf.Sin(angle) + input.x * Mathf.Cos(angle), input.y * Mathf.Cos(angle) + input.x * Mathf.Sin(angle)) : input;
        float rotation = input.z * rotationSpeed;
        
        // strafe = -input.y * Mathf.Sin(angle) + input.x * Mathf.Cos(angle);
        // forward = input.y * Mathf.Cos(angle) + input.x * Mathf.Sin(angle);

        float a = translation.x - rotation * length / 2;
        float b = translation.x + rotation * length / 2;
        float c = translation.y - rotation * width / 2;
        float d = translation.y + rotation * width / 2;
        
        frontRightSpeed = GetHyp(b, c);
        frontRightAngle = Mathf.Atan2(b, c) * Mathf.Rad2Deg;
        
        frontLeftSpeed = GetHyp(b, d);
        frontLeftAngle = Mathf.Atan2(b, d) * Mathf.Rad2Deg;
        
        backRightSpeed = GetHyp(a, c);
        backRightAngle = Mathf.Atan2(a, c) * Mathf.Rad2Deg;
        
        backLeftSpeed = GetHyp(a, d);
        backLeftAngle = Mathf.Atan2(a, d) * Mathf.Rad2Deg;
        
        float max = Mathf.Max(frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed);

        if (max <= 1) return;
        frontRightSpeed /= max;
        frontLeftSpeed /= max;
        backRightSpeed /= max;
        backLeftSpeed /= max;
    }

    private static float GetHyp(float a, float b)
    {
        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }

    private static Vector3 V2ToV3(Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(V2ToV3(translation)));

        if (currentNodePos != null) Gizmos.DrawWireSphere(currentNodePos.Value, 0.1f);
    }

    public void SetRotation(float angle)
    {
        robotBody.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
