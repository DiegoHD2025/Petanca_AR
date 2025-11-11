using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ReticleController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject reticlePrefab; // Prefab (ReticleRoot)
    public Camera arCamera;          // AR Camera (arrástrala)
    public float reticleScale = 0.06f;
    public bool stickToPlane = true; // si true, orienta la retícula al plano

    ARRaycastManager _raycastManager;
    GameObject _reticleInstance;
    static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();

        if (reticlePrefab != null)
        {
            _reticleInstance = Instantiate(reticlePrefab);
            _reticleInstance.SetActive(false);
        }

        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
    }

    void Update()
    {
        if (_reticleInstance == null || arCamera == null) return;

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // Raycast: solo contra planos dentro del polígono detectado
        if (_raycastManager.Raycast(screenCenter, _hits, TrackableType.PlaneWithinPolygon))
        {
            // toma el primer hit (el más cercano)
            var hit = _hits[0];
            Pose hitPose = hit.pose;

            // posiciona retícula
            _reticleInstance.SetActive(true);
            _reticleInstance.transform.position = hitPose.position;

            // escala ajustable: puedes escalar según la distancia si quieres
            _reticleInstance.transform.localScale = Vector3.one * reticleScale;

            if (stickToPlane)
            {
                // orienta para que quede plana sobre el plano detectado
                _reticleInstance.transform.rotation = hitPose.rotation;
            }
            else
            {
                // billboard hacia la cámara, manteniendo ejes X/Z planos
                Vector3 look = arCamera.transform.position - _reticleInstance.transform.position;
                look.y = 0; // opcional: evita inclinación en Y
                if (look.sqrMagnitude > 0.001f)
                    _reticleInstance.transform.rotation = Quaternion.LookRotation(look);
            }

            // Si quieres anclar la retícula para que persista en seguimiento:
            // puedes usar ARAnchorManager aquí (opcional).
        }
        else
        {
            // no hit en plano -> ocultar retícula (zona no escaneada)
            if (_reticleInstance.activeSelf)
                _reticleInstance.SetActive(false);
        }
    }
}
