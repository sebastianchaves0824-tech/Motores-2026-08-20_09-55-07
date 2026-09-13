using UnityEngine;

public class SpawnSetter : MonoBehaviour
{
    public GameObject spawnPoint;
    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint.transform.position = newSpawnPoint.position;
    }
}
