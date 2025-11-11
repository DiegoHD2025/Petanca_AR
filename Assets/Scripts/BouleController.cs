using UnityEngine;

public class BouleController : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isDragging = false;
    private Rigidbody rb;
    private bool hasThrown = false;

    [Header("Tuning")]
    public float forceMultiplier = 0.03f;
    public float upwardForceFactor = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (hasThrown) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isDragging = true;
                startPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endPos = Input.mousePosition;
            ThrowBoule();
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    isDragging = true;
                    startPos = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                endPos = touch.position;
                ThrowBoule();
            }
        }
#endif
    }

    void ThrowBoule()
    {
        rb.isKinematic = false;

        Vector2 dragVector = endPos - startPos;

        // Dirección 3D en el espacio de la cámara
        Vector3 throwDirection =
            Camera.main.transform.forward * Mathf.Abs(dragVector.y) +
            Camera.main.transform.right * dragVector.x +
            Camera.main.transform.up * (dragVector.magnitude * upwardForceFactor);

        throwDirection.Normalize();

        rb.AddForce(throwDirection * dragVector.magnitude * forceMultiplier, ForceMode.Impulse);

        hasThrown = true;
        isDragging = false;
    }
}
