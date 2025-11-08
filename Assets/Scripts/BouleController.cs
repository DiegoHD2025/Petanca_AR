using UnityEngine;

public class BouleController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isDragging = false;
    private Rigidbody rb;
    private bool hasThrown = false;
    public float forceMultiplier = 0.02f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isDragging = true;
                startPos = mousePos;
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
        Vector3 dir = (endPos - startPos);
        Vector3 throwDir = new Vector3(dir.x, dir.magnitude * 0.1f, dir.y).normalized;

        rb.isKinematic = false;
        rb.AddForce(throwDir * dir.magnitude * forceMultiplier, ForceMode.Impulse);

        hasThrown = true;
        isDragging = false;
    }
}
