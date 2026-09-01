using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float gravityFactor;
    private Vector3 movementDirection;
    private bool isGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // leemos el input del teclado y lo guardamos en un vector3
        movementDirection = playerInput.actions["Movement"].ReadValue<Vector3>();
        movementDirection.Normalize();
        GroundCheck();
        Debug.Log("La variable está en el piso es " + isGrounded);
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
        rb.linearVelocity = new Vector3(newVelocity.x, actualVelocity.y, newVelocity.z);

        if (rb.linearVelocity.y < -1f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * gravityFactor, rb.linearVelocity.z);
        }
        else if (rb.linearVelocity.y > 6f)
        {

        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.performed && isGrounded)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

    }
    private void GroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 1.5f, Color.green);
        if (Physics.Raycast(ray, 1.5f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}