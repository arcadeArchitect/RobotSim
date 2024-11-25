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
        if (!pathManager || !pathManager.editPath) return;

        foreach (var node in pathManager.nodes)
        {
            var newPosition = node.position;
            var newRotation = Quaternion.Euler(0, -node.rotation, 0);
            Handles.TransformHandle(ref newPosition, ref newRotation);

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
}