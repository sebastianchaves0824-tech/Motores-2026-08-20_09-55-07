using UnityEngine;

public class Killers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. Intentamos quitar vida mediante el nuevo sistema
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.RecibirDanio(1);
            return;
        }

        // 2. Soporte por si otro objeto antiguo todavía usa Death.cs
        Death player = other.GetComponent<Death>();
        if (player != null)
        {
            player.Die();
        }
    }
}