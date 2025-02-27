using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathManager))]
public class PathManagerUI : Editor
{
    private PathManager pathManager;

    public void OnEnable()
    {
        pathManager = target as PathManager;
    }

    public void OnSceneGUI()
    {
        if (!pathManager) return;

        if (pathManager.editPath)
        {
            foreach (var node in pathManager.nodes)
            {
                var newPosition = node.position;
                var newRotation = Quaternion.Euler(0, -node.rotation, 0);
                Handles.TransformHandle(ref newPosition, ref newRotation);
            
                // var newPosition = Handles.PositionHandle(node.position, quaternion.identity);
            
                if (newPosition != node.position)
                {
                    newPosition.y = 0;
                    node.position = newPosition;
                }
            
                if (!Mathf.Approximately(newRotation.eulerAngles.y, node.rotation))
                {
                    node.SetRotation(-newRotation.eulerAngles.y);
                }
            }
        }

        if (pathManager.editControlPoints)
        {
            foreach (var node in pathManager.nodes)
            {

                if (node.display != ControlDisplay.Two)
                {
                    var newControlPointOnePosition = Handles.PositionHandle(node.controlPointOne, quaternion.identity);

                    if (newControlPointOnePosition != node.controlPointOne)
                    {
                        node.SetGlobalControlPointOne(newControlPointOnePosition);
                    }
                }
                
                if (node.display != ControlDisplay.One)
                {
                    var newControlPointTwoPosition = Handles.PositionHandle(node.controlPointTwo, quaternion.identity);

                    if (newControlPointTwoPosition != node.controlPointTwo)
                    {
                        node.SetGlobalControlPointTwo(newControlPointTwoPosition);
                    }
                }
            }
        }
    }
}