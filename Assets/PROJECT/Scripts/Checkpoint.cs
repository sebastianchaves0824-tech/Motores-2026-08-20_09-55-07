using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SpawnSetter player = other.GetComponent<SpawnSetter>();
        SoundFXManager.Instance.Play3DSound("CheckpointClip", transform.position, volume: 1f);
        if (player != null)
        {
            player.SetSpawnPoint(transform);
        }
    }
}
