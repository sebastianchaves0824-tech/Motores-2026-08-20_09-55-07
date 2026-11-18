using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Particulas")]

    [SerializeField] ParticleSystem particulaSalto;
    [Header("Movimiento")]
    public float maxSpeed = 10f;
    public float acceleration = 60f;
    public float deceleration = 50f;
    [Tooltip("Qué tan rápido gira la Mulita hacia la dirección de avance")]
    public float turnSpeed = 15f;
    
    [Header("Animación y Transformación")]
    [Tooltip("Arrastrá el componente Animator del Player")]
    [SerializeField] private Animator animator;

    [Tooltip("Velocidad mínima en el plano horizontal para transformarse en esfera")]
    [SerializeField] private float umbralMovimiento = 0.2f;

    [Header("Salto")]
    public float jumpForce = 12f;
    [Range(0f, 1f)]
    [SerializeField] private float airControl = 0.6f;

    [Header("Físicas y Gravedad")]
    [SerializeField] private float gravityScale = 2.5f;
    [SerializeField] private float fallMultiplier = 2f;

    [Header("Ajustes de Pendiente")]
    [Tooltip("Multiplicador extra de velocidad al bajar una pendiente")]
    [SerializeField] private float bonusBajada = 1.5f;

    [Tooltip("Porcentaje de velocidad retenida al subir (0.4 = 40%)")]
    [Range(0.1f, 1f)]
    [SerializeField] private float penalizacionSubida = 0.4f;

    private Vector3 groundNormal = Vector3.up;
    private bool isJumping;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 movementDirection;
    private bool isGrounded;
    private bool estaEnHielo = false;
    private float desaceleracionPendienteRestaurar = -1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        movementDirection = new Vector3(input.x, 0f, input.y).normalized;

        GroundCheck();
        ActualizarAnimacion();
    }

    private void ActualizarAnimacion()
    {
        if (animator == null || rb == null) return;

        Vector3 velocidadPlana = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool estaMoviendose = velocidadPlana.magnitude > umbralMovimiento;

        animator.SetBool("EsBola", estaMoviendose);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotarHaciaMovimiento();
        CustomGravity();
    }

    private void RotarHaciaMovimiento()
    {
        // Solo rotamos si el jugador está presionando alguna dirección
        if (movementDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }

    private void MovePlayer()
    {
        Vector3 actualVelocity = rb.linearVelocity;
        float currentMaxSpeed = CalcularVelocidadPorPendiente();
        Vector3 targetVelocity = movementDirection * currentMaxSpeed;

        float velocityChange = (movementDirection.magnitude > 0.01f) ? acceleration : deceleration;

        if (!isGrounded)
        {
            velocityChange *= airControl;
        }

        Vector3 newVelocity = Vector3.MoveTowards(actualVelocity, targetVelocity, velocityChange * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(newVelocity.x, actualVelocity.y, newVelocity.z);
    }

    public void SetDeceleration(float nuevoValor)
    {
        deceleration = nuevoValor;
        estaEnHielo = true;
        desaceleracionPendienteRestaurar = -1f;
    }

    public void SalirDeSuperficieHielo(float valorNormal)
    {
        estaEnHielo = false;

        if (!isGrounded)
        {
            desaceleracionPendienteRestaurar = valorNormal;
        }
        else
        {
            deceleration = valorNormal;
        }
    }

    private void CustomGravity()
    {
        float baseGravity = Physics.gravity.y;
        float customGravity = baseGravity * (gravityScale - 1f);

        if (rb.linearVelocity.y < 0)
        {
            customGravity = baseGravity * (fallMultiplier - 1f);
        }
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            customGravity = baseGravity * (fallMultiplier - 1f);
        }

        Vector3 newGravity = new Vector3(0, customGravity * Time.fixedDeltaTime, 0);
        rb.linearVelocity += newGravity;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            isJumping = true;
            crearParticulaSalto();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (SoundFXManager.Instance != null)
            {
                SoundFXManager.Instance.Play3DSound("JumpClip", transform.position, volume: 1f);
            }
        }
        if (context.canceled)
        {
            isJumping = false;
        }
    }

    private void GroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 1.1f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.1f))
        {
            if (!isGrounded && desaceleracionPendienteRestaurar > 0f && !estaEnHielo)
            {
                deceleration = desaceleracionPendienteRestaurar;
                desaceleracionPendienteRestaurar = -1f;
            }

            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
    }

    private float CalcularVelocidadPorPendiente()
    {
        if (!isGrounded || groundNormal == Vector3.up)
        {
            return maxSpeed;
        }

        Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;
        float slopeDot = Vector3.Dot(movementDirection, slopeDirection);

        if (slopeDot > 0.05f)
        {
            return maxSpeed * Mathf.Lerp(1f, bonusBajada, slopeDot);
        }
        else if (slopeDot < -0.05f)
        {
            return maxSpeed * Mathf.Lerp(1f, penalizacionSubida, -slopeDot);
        }

        return maxSpeed;
    }
    private void crearParticulaSalto()
    {
        particulaSalto.Play();
    }
}