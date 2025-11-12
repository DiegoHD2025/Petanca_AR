using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaneColliderSetup : MonoBehaviour
{
    private ARPlaneManager planeManager;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDestroy()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            AddColliderToPlane(plane);
        }

        foreach (var plane in args.updated)
        {
            AddColliderToPlane(plane);
        }
    }

    private void AddColliderToPlane(ARPlane plane)
    {
        var meshCollider = plane.GetComponent<MeshCollider>();
        var meshFilter = plane.GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.mesh != null)
        {
            if (meshCollider == null)
                meshCollider = plane.gameObject.AddComponent<MeshCollider>();

            meshCollider.sharedMesh = meshFilter.mesh;
        }
    }
}
