using System;
using UnityEngine;

public class ReplaceNotes : MonoBehaviour
{
    [SerializeField] private bool replace;
    
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform[] notesToReplace;

    private void OnValidate()
    {
        if (!replace) return;
        
        Replace();
        replace = false;
    }

    private void Replace()
    {
        foreach (Transform note in notesToReplace)
        {
            Instantiate(notePrefab, note.position, note.rotation, parent);
            Destroy(note);
        }
    }
}
