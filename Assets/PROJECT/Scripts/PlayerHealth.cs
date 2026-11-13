using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Referencia a la UI")]
    [SerializeField] private HeartUI heartUI;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageVoiceLines; // Array para poner varias frases de daño
    [SerializeField] private AudioMixerGroup grupoMixer;
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

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null && grupoMixer != null)
        {
            audioSource.outputAudioMixerGroup = grupoMixer;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        PlayDamageVoiceLine();

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
    private void PlayDamageVoiceLine()
    {
        // Verificamos que haya clips asignados y que el AudioSource no esté sonando ya 
        if (damageVoiceLines != null && damageVoiceLines.Length > 0)
        {

            if (audioSource.isPlaying) return;

            // Seleccionamos un clip al azar del array
            int randomIndex = Random.Range(0, damageVoiceLines.Length);
            AudioClip clipToPlay = damageVoiceLines[randomIndex];

            // Reproducimos el clip usando el AudioSource configurado con el Audio Mixer
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}