using UnityEngine;

[System.Serializable]
public class Node
{
    public Node previousNode { get; private set; }
    public Node nextNode { get; private set; }
    public Vector3 position;
    public float rotation;
    public int index { get; private set; }

    public void SetPreviousNode(Node node)
    {
        previousNode = node;
    }

    public void SetNextNode(Node node)
    {
        nextNode = node;
    }

    public float SetRotation(float angle)
    {
        angle %= 360;
        rotation = (angle > 180) ? angle - 360 : angle;
        return rotation;
    }

    public float GetDistanceToNextNode()
    {
        return Vector3.Distance(position, nextNode.position);
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public bool IsFirstNode()
    {
        return index == 0;
    }

    public bool IsLastNode()
    {
        return index == -1;
    }
}