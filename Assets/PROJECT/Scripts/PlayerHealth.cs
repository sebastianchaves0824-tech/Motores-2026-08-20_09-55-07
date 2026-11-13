using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Referencia a la UI")]
    [SerializeField] private HeartUI heartUI;

    public UnityEvent onDeath;

    private Vector3 puntoSpawnActual;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        puntoSpawnActual = transform.position;

        if (heartUI != null)
        {
            heartUI.UpdateHearts(currentHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (heartUI != null)
        {
            heartUI.UpdateHearts(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Si perdió vida pero aún le quedan corazones, vuelve al último checkpoint
            RespawnearEnCheckpoint();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (heartUI != null)
        {
            heartUI.UpdateHearts(currentHealth);
        }
    }

    // Método invocado por SpawnSetter.cs al tocar un checkpoint
    public void SetPuntoSpawn(Transform nuevoPunto)
    {
        if (nuevoPunto != null)
        {
            puntoSpawnActual = nuevoPunto.position;
            Debug.Log($"Checkpoint actualizado: {puntoSpawnActual}");
        }
    }

    public void RespawnearEnCheckpoint()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = puntoSpawnActual;
    }

    private void Die()
    {
        Debug.Log("El jugador murió.");
        onDeath?.Invoke();
    }
    public void CargarEscenaDerrota(string nombreEscena)
{
    SceneManager.LoadScene(nombreEscena);
}
}