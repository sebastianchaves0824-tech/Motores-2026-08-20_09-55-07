using UnityEngine;

public class TriggerPersecusion : MonoBehaviour
{
    public BolaAsesina bola;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bola.StartChase();
            gameObject.SetActive(false); 
        }
    }
}
