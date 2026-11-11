using UnityEngine;

public class BolaAsesina : MonoBehaviour
{
    [Header("Ruta y Objetivos")]
    [Tooltip("Lista de puntos que seguirá la bola en orden.")]
    public Transform[] waypoints;

    [Header("Ajustes de Velocidad")]
    public float baseSpeed = 10f;
    public float maxSpeed = 20f;
    public float acceleration = 2f;
    public float rotationMultiplier = 500f;

    [Header("Referencia al Jugador")]
    public Transform player;
    public float killDistance = 1.5f;

    private int currentWaypointIndex = 0;
    private float currentSpeed;
    private bool isChasing = false;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!isChasing || waypoints.Length == 0) return;

        MoverBola();
        RotarVisualmente();
        VerificarContactoConJugador();
    }

    // Inicia la persecución (puedes llamarlo con un Trigger)
    public void StartChase()
    {
        isChasing = true;
    }

    void MoverBola()
    {
        // 1. Aumentar la velocidad paulatinamente
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        // 2. Obtener el objetivo actual
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // 3. Mover la posición hacia el punto objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            currentSpeed * Time.deltaTime
        );

        // 4. Cambiar al siguiente punto al llegar
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Llegó al final del camino
                isChasing = false;
            }
        }
    }

    void RotarVisualmente()
    {
        // Hace girar la esfera hacia adelante según su velocidad de movimiento
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
            transform.Rotate(rotationAxis, currentSpeed * rotationMultiplier * Time.deltaTime, Space.World);
        }
    }

    void VerificarContactoConJugador()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killDistance)
        {
            // Lógica de muerte del jugador
            Debug.Log("¡La bola aplastó al jugador!");
            // player.GetComponent<PlayerHealth>().Die();
        }
    }
}
