using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    //cuanto tiempo tarda en caer la plataforma
    public float fallDelay = 1f;
    //cuanto tiempo tarda en reaparecer la plataforma
    public float respawnDelay = 3f;
    //velocidad a la que cae la plataforma
    public float fallSpeed = 17f; //numero entre 10 y 20

    //referencia al rigidbody y collider
    private Rigidbody rb;
    private Collider col;

    //posicion y rotacion inicial de la plataforma
    private Vector3 startPosition;
    private Quaternion startRotation;

    //booleano que indica si ya ha caido, evita multiples activaciones de la plataforma
    private bool hasFallen = false;
    //boleano que indica si esta cayendo, controla cuando aplicar la velocidad de caida
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        rb.isKinematic = true;
    }

    //si el player(u objeto con el tag de player) entra en la plataforma, se activa la corrutina
    private void OnTriggerEnter(Collider other) 
    {
        if (hasFallen) return;

        if (other.CompareTag("Player")) 
        {
            StartCoroutine(FallPlatform());
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //esto hace que caiga hacia abajo con la velocidad que establecimos
        if (isFalling) 
        { 
          rb.linearVelocity = Vector3.down * fallSpeed;
        } 
    }

    //corrutina
    IEnumerator FallPlatform() 
    {
        if (hasFallen) yield break;

        hasFallen = true;

        yield return new WaitForSeconds(fallDelay);

        col.enabled = false;

        rb.isKinematic = false;
        rb.useGravity = false;

        isFalling = true;

        yield return new WaitForSeconds(respawnDelay);

        ResetPlatform();
    }

    //esta funcion devuelve la plataforma a su estado original despues de haber caido
    void ResetPlatform() 
    {
        isFalling = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;

        transform.position = startPosition;
        transform.rotation = startRotation;

        col.enabled = true;
        hasFallen = false;
    }
}
