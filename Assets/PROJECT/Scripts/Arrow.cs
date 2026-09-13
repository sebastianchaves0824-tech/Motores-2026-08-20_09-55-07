using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * velocidad;
    }
    void Update()
    {
        
    }
        void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
}
