using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    public bool editPath = false;
    public List<Node> nodes = new();
    private List<Node> path = new();
    [SerializeField] private int pathDivisions = 2;
    [SerializeField] private Transform robotChassis;
    [SerializeField] private Vector3 chassisScale;
    [SerializeField] private float chassisYOffset = 0.2f;

    public void Update()
    {
        ConfigureNodes();
    }

    private void ConfigureNodes()
    {
        Node previousNode = null;
        for (int i = 0; i < nodes.Count; ++i)
        {
            Node node = nodes[i];
            
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
    }

    private void OnDrawGizmos()
    {
        foreach (Node node in nodes)
        {
            Gizmos.color = node.IsFirstNode() ? Color.cyan : node.IsLastNode() ? Color.red : Color.green;
            
            // Gizmos.DrawSphere(node.position, 0.05f);
            Gizmos.DrawWireCube(node.position + Vector3.up * chassisYOffset, chassisScale);
            

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(node.position, new Vector3(Mathf.Cos(node.rotation * Mathf.Deg2Rad), 0, Mathf.Sin(node.rotation * Mathf.Deg2Rad)));
            
            if (node.nextNode == null) return;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(node.position, node.nextNode.position);
        }
    }
}
