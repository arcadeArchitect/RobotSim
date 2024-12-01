using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    public bool editPath;
    public bool editControlPoints;
    [SerializeField] private bool drawControls = true;
    public List<BezierNode> nodes = new();
    public List<Node> path = new();
    [Range(1, 50)] [SerializeField] private int pathDivisions = 3;
    [SerializeField] private bool showPathNodes;
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
            ++index;

            if (node.IsLastNode())
            {
                node.SetIndex(-1);
                path.Add(node);
                continue;
            }
            
            path.Add(node);
            
            Vector3 a = node.position;
            Vector3 b = node.controlPointTwo;
            Vector3 c = ((BezierNode)node.nextNode).controlPointOne;
            Vector3 d = node.nextNode.position;

            for (float i = spacing; i < 1; i += spacing)
            {
                Vector3 nodePosition = Mathf.Pow(1 - i, 3) * a + 3 * Mathf.Pow(1 - i, 2) * i * b + 3 * (1 - i) * Mathf.Pow(i, 2) * c + Mathf.Pow(i, 3) * d;
                Node newNode = new Node(nodePosition, index);
                
                path.Add(newNode);
            }

            ++index;
        }
        
        for (int i = 0; i < path.Count; ++i)
        {
            Node node = path[i];
            if (!node.IsFirstNode()) node.SetPreviousNode(path[i - 1]);
            if (!node.IsLastNode()) node.SetNextNode(path[i + 1]);
        }
    }

    private void OnDrawGizmos()
    {
        
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
        }
    }
}
