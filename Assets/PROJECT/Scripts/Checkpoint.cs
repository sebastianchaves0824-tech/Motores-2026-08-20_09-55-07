using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SpawnSetter player = other.GetComponent<SpawnSetter>();
        if (player != null)
        {
            player.SetSpawnPoint(transform);
        }
    }
}
