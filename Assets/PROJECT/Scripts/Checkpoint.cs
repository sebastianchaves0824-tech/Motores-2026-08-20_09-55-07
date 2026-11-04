using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform puntoSpawnPersonalizado;

    private void OnTriggerEnter(Collider other)
    {
        SpawnSetter player = other.GetComponent<SpawnSetter>();
        if (player != null)
        {
            // Si no asignas nada en el inspector, usa el propio transform por defecto
            Transform destino = puntoSpawnPersonalizado != null ? puntoSpawnPersonalizado : transform;
            player.SetSpawnPoint(destino);
        }
    }
}
