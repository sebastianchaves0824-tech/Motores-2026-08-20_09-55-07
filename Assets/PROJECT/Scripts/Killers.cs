using UnityEngine;

public class Killers : MonoBehaviour
{
    [SerializeField] private int danio = 1;

    private void OnTriggerEnter(Collider other)
    {
        
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(danio);
            return;
        }

       
        Death player = other.GetComponent<Death>();
        if (player != null)
        {
            player.Die();
        }
    }
}