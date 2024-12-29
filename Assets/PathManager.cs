using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    [FormerlySerializedAs("showPath")] public bool drawGizmos = true;
    public bool editPath;
    public bool editControlPoints;
    [SerializeField] private bool drawControls = true;
    public List<BezierNode> nodes = new();
    public List<Node> path = new();
    [Range(1, 100)] [SerializeField] private int pathDivisions = 3;
    [SerializeField] private bool showPathNodes;
    public bool showDerivatives;
    [SerializeField] private Transform robotChassis;
    [SerializeField] private Vector3 chassisScale;
    [SerializeField] private float chassisYOffset = 0.2f;

    public void Update()
    {
        ConfigureNodes();
        GeneratePath();
    }

    private void ConfigureNodes()
    {
        BezierNode previousNode = null;
        for (int i = 0; i < nodes.Count; ++i)
        {
            BezierNode node = nodes[i];
            
            node.SetPreviousNode(previousNode);

            if (i + 1 < nodes.Count)
            {
                node.SetIndex(i);
                node.SetNextNode(nodes[i + 1]);
            }
            else
            {
                node.SetIndex(-1);
            }
            
            previousNode = node;
        }

        Vector3 lastPosition = Vector3.negativeInfinity;
        foreach (BezierNode node in nodes)
        {
            if (node.position == lastPosition) node.position += Vector3.right + Vector3.forward;
            lastPosition = node.position;
            
            if (node.controlOffset == Vector3.zero) node.SetLocalControlPointOne(new Vector3(0.5f, 0, 0.5f));
            
            node.FixRotation();
            node.UpdateControlPoints();
        }
    }

    private void GeneratePath()
    {
        path = new List<Node>();
        
        int index = 0;
        
        float spacing = 1 / (float)(pathDivisions + 1);
        foreach (BezierNode node in nodes)
        {
            Node newBezierNode = new Node(node.position, node.index);
            newBezierNode.SetRotation(node.rotation);

            if (node.IsLastNode())
            {
                newBezierNode.SetIndex(-1);
                path.Add(newBezierNode);
                continue;
            }
            
            path.Add(newBezierNode);
            
            Vector3 a = node.position;
            Vector3 b = node.controlPointTwo;
            Vector3 c = ((BezierNode)node.nextNode).controlPointOne;
            Vector3 d = node.nextNode.position;



            node.vel = (-3 * a + 3 * b);
            
            for (float t = spacing; t < 1; t += spacing)
            {

                ++index;
                // Vector3 nodePosition = Mathf.Pow(1 - t, 3) * a + 3 * Mathf.Pow(1 - t, 2) * t * b + 3 * (1 - t) * Mathf.Pow(t, 2) * c + Mathf.Pow(t, 3) * d;
                Vector3 nodePosition = a + t * (-3 * a + 3 * b) + Mathf.Pow(t, 2) * (3 * a - 6 * b + 3 * c) +
                                       Mathf.Pow(t, 3) * (-a + 3 * b - 3 * c + d);
                
                Vector3 nodeVelocity = (-3 * a + 3 * b) + 2 * t * (3 * a - 6 * b + 3 * c) +
                                       3 * Mathf.Pow(t, 2) * (-a + 3 * b - 3 * c + d);
                
                Node newNode = new Node(nodePosition, index)
                {
                    bezierIndex = index,
                    tVal = spacing,
                    vel = nodeVelocity,
                };

                path.Add(newNode);
            }
        }
        
        for (int i = 0; i < path.Count; ++i)
        {
            Node node = path[i];
            if (!node.IsFirstNode()) node.SetPreviousNode(path[i - 1]);
            if (!node.IsLastNode()) node.SetNextNode(path[i + 1]);
        }
    }

    public Vector3 PathVel(float tVal)
    {
        BezierNode node = nodes[(int)tVal];
        
            
        Vector3 a = node.position;
        Vector3 b = node.controlPointTwo;
        Vector3 c = ((BezierNode)node.nextNode).controlPointOne;
        Vector3 d = node.nextNode.position;

        float t = tVal - (int)tVal;
                
        Vector3 nodeVelocity = (-3 * a + 3 * b) + 2 * t * (3 * a - 6 * b + 3 * c) +
                               3 * Mathf.Pow(t, 2) * (-a + 3 * b - 3 * c + d);
        
        return nodeVelocity;
    }

    public Vector3? PathPoint(float tVal)
    {
        BezierNode node = nodes[(int)tVal];

        if ((BezierNode)node.nextNode == null) return null;
        
            
        Vector3 a = node.position;
        Vector3 b = node.controlPointTwo;
        Vector3 c = ((BezierNode)node.nextNode).controlPointOne;
        Vector3 d = node.nextNode.position;

        float t = tVal - (int)tVal;
        
        Vector3 nodePosition = a + t * (-3 * a + 3 * b) + Mathf.Pow(t, 2) * (3 * a - 6 * b + 3 * c) +
                               Mathf.Pow(t, 3) * (-a + 3 * b - 3 * c + d);
        
        return nodePosition;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        foreach (BezierNode node in nodes)
        {
            if (!drawControls) break;
            
            Gizmos.color = node.IsFirstNode() ? Color.cyan : node.IsLastNode() ? Color.red : Color.green;
            
            // Gizmos.DrawSphere(node.position, 0.05f);
            Gizmos.DrawWireCube(node.position + Vector3.up * chassisYOffset, chassisScale);
            

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(node.position, new Vector3(Mathf.Cos(node.rotation * Mathf.Deg2Rad), 0, Mathf.Sin(node.rotation * Mathf.Deg2Rad)));
            
            Gizmos.color = Color.yellow;
            if (node.display != ControlDisplay.Two)
            {
                Gizmos.DrawWireSphere(node.controlPointOne, 0.05f);
                Gizmos.DrawLine(node.controlPointOne, node.position);
            }

            if (node.display != ControlDisplay.One)
            {
                Gizmos.DrawWireSphere(node.controlPointTwo, 0.05f);
                Gizmos.DrawLine(node.controlPointTwo, node.position);
            }
            
            // if (node.nextNode == null) return;
            // Gizmos.color = Color.white;
            // Gizmos.DrawLine(node.position, node.nextNode.position);
        }
        
        foreach (Node node in path)
        {
            if (showPathNodes) Gizmos.DrawWireSphere(node.position, 0.025f);
            if (node.nextNode == null) return;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(node.position, node.nextNode.position);
            if (showDerivatives) Gizmos.DrawRay(node.position, node.vel);
        }
    }
}
