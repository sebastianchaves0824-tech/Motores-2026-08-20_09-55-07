using UnityEngine;

public class Killers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Death player = other.GetComponent<Death>();
        if (player != null)
        {
            player.Die();
        }
    }
}
