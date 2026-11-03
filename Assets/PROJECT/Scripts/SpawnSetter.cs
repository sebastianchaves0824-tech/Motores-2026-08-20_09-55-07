using UnityEngine;

public class SpawnSetter : MonoBehaviour
{
    public GameObject spawnPoint;

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        // 1. Mantiene la posición del GameObject que usabas como punto de referencia
        if (spawnPoint != null)
        {
            spawnPoint.transform.position = newSpawnPoint.position;
        }

        // 2. Sincroniza el punto de reaparición en el sistema de vidas del jugador
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetPuntoSpawn(newSpawnPoint);
        }
    }
}