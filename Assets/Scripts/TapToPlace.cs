using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TapToPlace : MonoBehaviour
{
    public GameManager gameManager;
    public ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool bolichePlaced = false;

    void Update()
    {
#if UNITY_EDITOR
        // Simulación en editor con clic de ratón
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandlePlacement(hit.point);
            }
        }
#else
        // En build real con pantalla táctil
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    HandlePlacement(hitPose.position);
                }
            }
        }
#endif
    }

    void HandlePlacement(Vector3 position)
    {
        if (!bolichePlaced)
        {
            gameManager.SpawnBoliche(position + Vector3.up * 0.05f);
            bolichePlaced = true;
        }
        else
        {
            gameManager.SpawnBoule(position + Vector3.up * 0.05f);
        }
    }
}
