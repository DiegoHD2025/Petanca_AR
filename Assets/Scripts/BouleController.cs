using UnityEngine;

public class BouleController : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isDragging = false;
    private Rigidbody rb;
    public Transform reticle; // referencia al blanco en el mundo AR

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
        if (rb.velocity.magnitude > 0.3f) return;

        if (!rb.isKinematic && rb.velocity.magnitude < 0.05f)
        {
            rb.isKinematic = true;
        }

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

        // Si tenemos un blanco (retícula), lanzamos hacia él
        Vector3 throwDirection;
        if (reticle != null)
        {
            // dirección desde la boule hacia la retícula
            throwDirection = (reticle.position - transform.position).normalized;

            // añade algo de fuerza vertical proporcional al arrastre
            throwDirection += Vector3.up * (dragVector.magnitude * upwardForceFactor * 0.01f);
            throwDirection.Normalize();
        }
        else
        {
            // Si no hay retícula, lanza hacia adelante como antes
            throwDirection =
                Camera.main.transform.forward * Mathf.Abs(dragVector.y) +
                Camera.main.transform.right * dragVector.x +
                Camera.main.transform.up * (dragVector.magnitude * upwardForceFactor);

            throwDirection.Normalize();
        }

        rb.AddForce(throwDirection * dragVector.magnitude * forceMultiplier, ForceMode.Impulse);

        isDragging = false;
    }

}
