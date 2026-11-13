using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    [SerializeField] private int maxVidas = 3;
    [SerializeField] private float tiempoInvulnerabilidad = 1.5f;

    [Header("Respawn")]
    [SerializeField] private Transform puntoSpawn;

    [Header("Eventos")]
    public UnityEvent<int> onHealthChanged; // Avisa a la UI cuántas vidas quedan
    public UnityEvent onGameOver;           // Dispara pantalla de derrota

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageVoiceLines; // Array para poner varias frases de daño
    [SerializeField] private AudioMixerGroup grupoMixer;


    private int vidasActuales;
    private bool esInvulnerable = false;
    private Rigidbody rb;
    private Vector3 posicionInicial;

    public int VidasActuales => vidasActuales;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vidasActuales = maxVidas;
        posicionInicial = transform.position;
    }

    private void Start()
    {
        onHealthChanged?.Invoke(vidasActuales);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Asignamos el Audio Mixer Group al AudioSource si ambos existen
        if (audioSource != null && grupoMixer != null)
        {
            audioSource.outputAudioMixerGroup = grupoMixer;
        }
    }

    public void RecibirDanio(int cantidad = 1)
    {
        if (esInvulnerable || vidasActuales <= 0) return;

        vidasActuales -= cantidad;
        onHealthChanged?.Invoke(vidasActuales);

        PlayDamageVoiceLine();

        if (vidasActuales <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
            Reaparecer();
        }
    }

    private void Reaparecer()
    {
        // Detener la inercia acumulada antes de reaparecer
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Vector3 destino = (puntoSpawn != null) ? puntoSpawn.position : posicionInicial;
        transform.position = destino;
    }

    private void Morir()
    {
        // Bloquear movimiento
        PlayerMovement mov = GetComponent<PlayerMovement>();
        if (mov != null) mov.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        onGameOver?.Invoke();
        Debug.Log("<color=red>¡Game Over! La mulita se quedó sin vidas.</color>");
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        // Opcional: parpadeo visual del renderer aquí
        yield return new WaitForSeconds(tiempoInvulnerabilidad);
        esInvulnerable = false;
    }

    public void SetPuntoSpawn(Transform nuevoSpawn)
    {
        puntoSpawn = nuevoSpawn;
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