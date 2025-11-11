using UnityEngine;

public class BouleThrower : MonoBehaviour
{
    public GameObject boulePrefab;
    public float throwForce = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic o toque
        {
            ThrowBoule();
        }
    }

    void ThrowBoule()
    {
        GameObject boule = Instantiate(boulePrefab, transform.position + transform.forward * 0.5f, Quaternion.identity);
        Rigidbody rb = boule.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
