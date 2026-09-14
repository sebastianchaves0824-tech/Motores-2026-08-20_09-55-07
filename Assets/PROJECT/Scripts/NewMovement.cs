using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    // poner el header hace que quede lindo en el inspector de Unity, pero no hace nada más
    [Header("Movimiento")]
    public float maxSpeed = 10f; // que tan rápido se va a mover la mulita
    public float acceleration = 60f; //
    public float deceleration = 50f;

    [Header("Salto")]
    public float jumpForce = 12f;
    [Range(0f, 1f)] // esto nos permite generar un slider en el inspector
    [SerializeField] private float airControl = 0.6f; // cuanto vamos a poder mover a nuestro personaje en el aire 

    [Header("Físicas y Gravedad")]
    [SerializeField] private float gravityScale = 2.5f;     // Gravedad base multiplicada
    [SerializeField] private float fallMultiplier = 2f;      // Gravedad extra al caer para evitar flotar

    [Header("Ajustes de Pendiente")]                         
    [Tooltip("Multiplicador extra de velocidad al bajar una pendiente")] 
    //muestra un tooltip en el inspector para saber que
    //modifica esta variable 
    [SerializeField] private float bonusBajada = 1.5f;        // Porcentaje de velocidad extra al bajar una pendiente (1.5 = 150%)

    [Tooltip("Porcentaje de velocidad retenida al subir (0.4 = 40%)")]
    //muestra un tooltip en el inspector para saber que
    //modifica esta variable

    [Range(0.1f, 1f)] // esto nos permite generar un slider en el inspector
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

    }

    private void Update()
    {
        // leemos el input del teclado y lo guardamos en un vector3
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        movementDirection = new Vector3(input.x, 0f, input.y).normalized;
        // normalizo el vector de entrada
        // ya que despues lo voy a multiplicar por la velocidad
        // de otra manera podria hacer que yendo en diagonal sea mas rapido 

        GroundCheck(); // en cada frame me fijo si está en el piso o no
    }

    private void FixedUpdate()
    {
        MovePlayer();
        CustomGravity();
    }

    private void MovePlayer()
    {
        Vector3 actualVelocity = rb.linearVelocity;
        float currentMaxSpeed = CalcularVelocidadPorPendiente();
        Vector3 targetVelocity = movementDirection * currentMaxSpeed; //definimos la velocidad deseada como el vector direccion por la magnitud que deseamos (maxspeed)
        // para cambiar la velocidad del personaje vamos a definir la velocidad máxima a la que queremos llegar y luego 
        // utilizar la funcion MoveTowards para que vaya llevando la velocidad hasta ese límite, con la tasa de cambio de velocityChange
        float velocityChange;
        if (movementDirection.magnitude > 0.01f)
        {
            velocityChange = acceleration;
        }
        else
        {
            velocityChange = deceleration;
        }
        // aca cambiamos la velocity change dependiendo si está saltando o en el piso
        if (!isGrounded)
        {
            velocityChange  = velocityChange * airControl; // al multiplicar por un número entre 0 y 1 lo que hacemos es reducir en ese porcentaje la tasa 
                                                           // de cambio de la velocidad, así el personaje se mueve mas lento en el aire
        }

        // acá conseguimos ese nuevo vector de velocidad
        Vector3 newVelocity = Vector3.MoveTowards(actualVelocity, targetVelocity, velocityChange * Time.fixedDeltaTime);

        // en esta linea cambiamos efectivamente la velocidad del rigidbody, conservando la velocidad vertical para no modificar el salto
        rb.linearVelocity = new Vector3(newVelocity.x, actualVelocity.y, newVelocity.z);
    }
        // Aca permitimos que superficies como el hielo modifiquen el frenado
    public void SetDeceleration(float nuevoValor)
    {
        deceleration = nuevoValor;
        estaEnHielo = true;
        desaceleracionPendienteRestaurar = -1f;
    }

    public void SalirDeSuperficieHielo(float valorNormal)
    {
        estaEnHielo = false;

        // Si salio del hielo porque salto, guarda el valor para restaurarlo recién cuando aterrice
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
        // guardamos el valor de la gravedad de unity
        float baseGravity = Physics.gravity.y;

        // aca elegimos la gravedad personalizada, usamos (gravityScale - 1) para que sea intuitivo de usar en el inspector para ir ajustando las variables
        // por eso si gravityScale es 2 entonces la gravedad personalizada será el doble que la de unity
        float customGravity = baseGravity * (gravityScale - 1f);

        // si el personaje está cayendo, le agregamos fuerza a la gravedad
        if (rb.linearVelocity.y < 0)
        {
            // acá modificamos la gravedad personalizada si estamos cayendo, tal cual hicimos antes pero en vez de gravityScale usamos fallMultiplier 
            customGravity = baseGravity * (fallMultiplier - 1f);
        }
        // acá lo que hacemos es añadirle gravedad en la subida si dejamos de apretar el botón de salto
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            // esto va a limitar la altura del salto, haciendo que apretar una vez salte menos que dejar apretado que salta completo
            customGravity = baseGravity * (fallMultiplier - 1f);
        }

        // en este paso sumamos la gravedad personalizada a la velocidad del rigidbody
        Vector3 newGravity = new Vector3(0,customGravity * Time.fixedDeltaTime,0);
        rb.linearVelocity = rb.linearVelocity + newGravity;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // ni bien apretamos el boton de salto comprobamos si está en el piso y si es así saltamos
        if (context.started && isGrounded)
        {
            isJumping = true;
            // antes de saltar ponemos la velocidad en 0, esto sirve para que si por alguna razon tenemos velocidad en y
            // por ejemplo al bajar una pendiente, o por una plataforma que nos mueve para arriba
            // no nos pase que el salto es distinto por eso, de esta manera siempre saltara lo mismo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (context.canceled)
        {
            isJumping = false;
        }
    } 
private void GroundCheck()// esta es una forma "sencilla" de determinar si el personaje esta en el piso y puede saltar o no
        // el raycast tira un rayo para abajo 
        // si detecta un objeto Physics.Raycast nos devuelve un booleano con valor true y si no detecta nada nos devuelve false
        // tambien nos permite obtener información del objeto que detecta, como la normal de la superficie, la distancia, etc
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 1.1f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.1f))
        {
            // Si estaba en el aire y acaba de aterrizar fuera del hielo, restaura la desaceleracion
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
    //aca vamos a calcular la velocidad máxima que puede tener el personaje 
    //dependiendo de si está subiendo o bajando una pendiente
    {
        if (!isGrounded || groundNormal == Vector3.up)
        {
            return maxSpeed;
        }

        // Vector hacia donde cae la colina
        Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;

        // Dot = +1 (Bajando la pendiente), Dot = -1 (Subiendo la pendiente)
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
}