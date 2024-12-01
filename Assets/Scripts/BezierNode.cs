using UnityEngine;

[System.Serializable]
public class BezierNode : Node
{
    
    [HideInInspector] public ControlDisplay display = ControlDisplay.Both;
    
    public Vector3 controlOffset = Vector3.zero;

    public BezierNode(Vector3 position, int index) : base(position, index) {}

    public Vector3 controlPointOne { get; private set; }
    public Vector3 controlPointTwo { get; private set; }
    
    public void SetGlobalControlPointOne(Vector3 point)
    {
        SetLocalControlPointOne(point - position);
    }
    
    public void SetGlobalControlPointTwo(Vector3 point)
    {
        SetGlobalControlPointOne(-point + 2 * position);
    }

    public void SetLocalControlPointOne(Vector3 point)
    {
        controlOffset = point;
        UpdateControlPoints();
    }

    public void UpdateControlPoints()
    {
        controlPointOne = position + controlOffset;
        controlPointTwo = position - controlOffset;
        
        display = IsFirstNode() ? ControlDisplay.Two : IsLastNode() ? ControlDisplay.One : ControlDisplay.Both;
    }
}

public enum ControlDisplay
{
    Both,
    One,
    Two
}