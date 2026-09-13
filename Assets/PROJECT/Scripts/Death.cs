using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject respawn;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //respawn = GetComponent<>();
    }

    public void Die()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = respawn.transform.position;
    }
}
