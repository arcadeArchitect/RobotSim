    using System.Collections.Generic;
    using UnityEngine;
    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

    public class ChainGenerator : MonoBehaviour
{
    [SerializeField] private Rigidbody linkPrefab;
    private Mesh linkMesh => linkPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 linkOffset;
    [SerializeField] private int chainLength;
    [SerializeField] private bool generate;
    
    private List<Rigidbody> chain;

    private void OnValidate()
    {
        if (!generate || chainLength < 1 || transform.childCount > 0) return;
        generate = false;
        chain = new List<Rigidbody>();

        CreateChain();
        LinkChain();
    }

    private void CreateChain()
    {
        for (int i = 0; i < chainLength; i++)
        {
            int r = i % 2;
            Rigidbody link = Instantiate(linkPrefab, GetLinkPosition(i), GetLinkRotation(r), transform);
            chain.Add(link);
            link.name = "Link - " + i;
        }
    }

    private void LinkChain()
    {
        for (int i = 0; i < chainLength; i++)
        {
            if (i == 0)
            {
                Rigidbody firstLink = chain[i];
                firstLink.isKinematic = true;
            }
            else
            {
                chain[i].GetComponent<HingeJoint>().connectedBody = chain[i - 1];
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (chainLength < 1 || transform.childCount > 0) return;
        
        Mesh mesh = linkMesh;

        Gizmos.color = Color.grey;
        for (int i = 0; i < chainLength; i++)
        {
            int r = i % 2;
            Gizmos.DrawWireMesh(mesh, GetLinkPosition(i), GetLinkRotation(r, true), transform.localScale);
        }
    }

    private Vector3 GetLinkPosition(int i)
    {
        return Multiply(transform.localScale, transform.position + transform.TransformDirection(startOffset + linkOffset * i));
    }

    private Quaternion GetLinkRotation(int r, bool gizmo = false)
    {
        return Quaternion.Euler(Vector3.up * (r * 90) + linkPrefab.transform.rotation.eulerAngles + (gizmo ? 90 * Vector3.forward : Vector3.zero));
    }
    
    private Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
