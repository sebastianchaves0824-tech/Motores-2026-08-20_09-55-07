using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CannonProjectile : MonoBehaviour
{
    [Header("Impacto")]
    [Tooltip("Fuerza con la que empuja al jugador al chocar")]
    [SerializeField] private float knockbackForce = 15f;

    [Tooltip("Tiempo de vida del proyectil antes de destruirse solo")]
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Dirección del impacto hacia el jugador con un leve impulso hacia arriba
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                pushDirection.y = 0.3f; 

                playerRb.AddForce(pushDirection.normalized * knockbackForce, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}