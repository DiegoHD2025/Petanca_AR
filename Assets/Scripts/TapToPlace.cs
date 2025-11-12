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
        // --- SIMULACIÓN EN EL EDITOR ---
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Si tocaste una Boule existente, no crear otra
                if (hit.collider.CompareTag("Boule"))
                    return;

                HandlePlacement(hit.point);
            }
        }
#else
    // --- EJECUCIÓN EN DISPOSITIVO MÓVIL ---
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Si tocaste una Boule existente, no crear otra
                if (hit.collider.CompareTag("Boule"))
                {
                    var boule = hit.collider.GetComponent<BouleController>();
                    if (boule != null)
                        return; // Evita crear nuevas
                }
            }

            // Si no tocaste una Boule, buscar plano AR
            if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
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
        // Coloca primero el boliche, luego las pelotas
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
