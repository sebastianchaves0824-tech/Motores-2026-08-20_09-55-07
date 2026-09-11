using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    //esta variable nos va a ayudar a controlar el alto del salto y la cantidad de tiempo en el aire
    [SerializeField] private float jumpForce;
    // esta variable va a guardar la direccion del movimiento al momento de apretar el boton de salto para guardarlo
    // y poder conservar esa direccion
    private Vector3 jumpDirection;
    // esta variable la vamos a usar para determinar cuanto se puede mover el personaje en el aire
    [SerializeField] private float jumpMovement;
    // esta va a ser la velocidad maxima
    [SerializeField] private float maxSpeed;
    // esta variable la vamos a usar para que el personaje se mueva mas rapido al moverse
    [SerializeField] private float acceleration;
    // esta variable la vamos a usar para que el personaje se detenga mas rapido, sin tanta inercia
    [SerializeField] private float deceleration;
    private Vector3 movementDirection;
    private bool isGrounded;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {
        // leemos el input del teclado y lo guardamos en un vector3
        movementDirection = playerInput.actions["Movement"].ReadValue<Vector3>();
        movementDirection.Normalize();
        GroundCheck();
        //Debug.Log("La variable está en el piso es " + isGrounded);
        // Normalizo el vector de entrada
        // ya que despues lo voy a multiplicar por la velocidad
        // de otra manera podria hacer que yendo en diagonal sea mas rapido 

    }

    private void FixedUpdate()
    {
        //aca me guardo la velocidad actual del rb
        Vector3 actualVelocity = rb.linearVelocity;
        //seteo cual es la velocidad maxima
        Vector3 targetVelocity = movementDirection * maxSpeed;

        // aca voy a ver si estamos moviendo al personaje o no
        // si lo muevo le pongo el valor que determine antes a esta nueva variable que va a ayudar
        // a que la aceleracion y desaceleracion sea mas rapida
        float velocityChange;
        if (movementDirection.magnitude > 0.01f)
        {
            velocityChange = acceleration;
        }
        else
        {
            velocityChange = deceleration;
        }

        // acá estoy creando un nuevo vector que se va a ir "incrementando" de acuerdo a lo que diga velocityChange
        Vector3 newVelocity = Vector3.MoveTowards(actualVelocity, targetVelocity, velocityChange * Time.fixedDeltaTime);

        //ahora hay que cambiarle la velocidad al rigidbody en linea con la del vector newVelocity
        // este if nos permite controlar diferentes movimientos dependiendo si el personaje esta en el suelo o saltando
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(newVelocity.x, actualVelocity.y, newVelocity.z);
        }
        else
        {
            jumpDirection = Vector3.MoveTowards(jumpDirection,targetVelocity,jumpMovement * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(jumpDirection.x, rb.linearVelocity.y, jumpDirection.z);
        }

    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.performed && isGrounded)
        {
            //aca guardo la direccion del salto
            jumpDirection = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            // aca le pongo velocidad en y para que salte
            rb.linearVelocity = new Vector3(jumpDirection.x, jumpForce, jumpDirection.z);
        }

    }
    private void GroundCheck()
    {
        //esta es una forma "sencilla" de determinar si el personaje esta en el piso y puede saltar o no
        // el raycast tira un rayo para abajo 
        // si detecta un objeto Physics.Raycast nos devuelve un booleano con valor true y si no detecta nada nos devuelve false
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 1f, Color.green);
        if (Physics.Raycast(ray, 1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}