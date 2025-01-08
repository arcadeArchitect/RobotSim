using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ClimbSystem : Subsystem
{
    [SerializeField] private GameObject robot;
    // public List<Grabbable> grabbables = new();
    private Grabbable grabby;
    private SpringJoint climbJoint;
    [SerializeField] private Transform castPos;
    [SerializeField] private float castRadius;
    private new Rigidbody rigidbody;

    public bool isClimbing { get; private set; }
    private Collider[] results;
    private Grabbable lastGrabby;
    [SerializeField] private ClimbAttach climbAttach;

    private Collider[] colliders;
    public Collider grabbyCollider;

    public Collider closestCollider;
    public Vector3 closestPoint;
    
    private void Start()
    {
        climbJoint = GetComponentInParent<SpringJoint>();
        climbAttach.robot = robot.GetComponent<Rigidbody>();
        colliders = robot.GetComponentsInChildren<Collider>();
        rigidbody = GetComponentInParent<Rigidbody>();
    }
    
    private void Grab()
    {
        // Debug.Log("GRAB");
        FindGrabbedObjects();
        if (grabby == null)
        {
            // Debug.Log("NVM GRAB");
            isClimbing = false;
            return;
        }
        
        // climbJoint = robot.AddComponent<SpringJoint>();
        // climbJoint.connectedArticulationBody = grabby.attachBody;
        climbAttach.target = grabby.transform;
        climbAttach.SetOffsetByWorldPos(grabbyCollider.ClosestPoint(castPos.position));
        grabby.attachedRigidbody = rigidbody;
        grabby.isBeingGrabbed = true;
        ChangeCollision(true);
        climbAttach.Attach();
    }

    private void FindGrabbedObjects()
    {
        results = Physics.OverlapSphere(castPos.position, castRadius);
        
        closestCollider = null;
        float closestDistance = float.MinValue;
        foreach (Collider c in results)
        {
            Grabbable g = c.GetComponentInParent<Grabbable>();
            closestPoint = c.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);
            if (!g) continue;
            if (!closestCollider || distance < closestDistance)
            {
                closestCollider = c;
                closestDistance = Vector3.Distance(transform.position, closestPoint);
                grabby = g;
                grabbyCollider = c;
            }
        }
        

        if (closestCollider)
        {
            closestPoint = grabbyCollider.ClosestPoint(transform.position);
            climbAttach.target = grabby.transform;
            climbAttach.SetOffsetByWorldPos(closestPoint);
            return;
        }
        grabby = null;
    }

    private void Ungrab()
    {
        // Debug.Log("UNGRAB");
        climbAttach.Detach();
        ChangeCollision(false);
        lastGrabby.isBeingGrabbed = false;
        grabby = null;
        // climbJoint.connectedArticulationBody = null;
        // Destroy(climbJoint);
    }

    public void ToggleGrab()
    {
        isClimbing = !isClimbing;
        if (isClimbing) Grab();
        else Ungrab();
    }

    private void Update()
    {
        if (!grabby) return;
        lastGrabby = grabby;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(closestPoint, 0.1f);
        
        if (!castPos) return;
        FindGrabbedObjects();
        Gizmos.color = isClimbing ? Color.Lerp(Color.blue, Color.cyan, 0.5f) : grabby ? Color.green : Color.red;
        Gizmos.DrawWireSphere(castPos.position, castRadius);
    }

    private void ChangeCollision(bool ignore)
    {
        foreach (Collider col in colliders)
            lastGrabby.ChangeCollision(col, ignore);
    }

    public void Climb(float input)
    {
        climbAttach.Move(Vector3.up * (input * Time.deltaTime));
    }
}
