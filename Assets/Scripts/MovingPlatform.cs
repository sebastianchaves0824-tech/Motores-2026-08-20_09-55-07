using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //array con los puntos a los que puede ir la plataforma
    public GameObject[] waypoints;

    // velocidad de la plataforma
    public float platformSpeed = 2;

    //index/lista de puntos
    private int waypointsIndex = 0;
   

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    //funcion para mover la plataforma
    void MovePlatform() 
    {
        //comprueba la distancia entre la plataforma y el punto que especifiquemos
        if (Vector3.Distance(transform.position, waypoints[waypointsIndex].transform.position) < 0.1f )
        {
            // sumamos un punto al index y vamos al siguiente punto/waypoint
            waypointsIndex++;

            if(waypointsIndex >= waypoints.Length) 
            { 
               // vuelve al primer punto, creando un bucle infinito
                waypointsIndex = 0;
            }
        
        }

        //posicion donde esta la plataforma(transform.position)
        //posicion a la que tiene que ir (waypoints[waypointsIndex].transform.position)
        //velocidad de la plataforma (platformSpeed multiplicada por deltatime para que vaya a la misma velocidad en todas las PCs)
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointsIndex].transform.position, platformSpeed * Time.deltaTime);


    }
    // PARA QUE EL PLAYER SIGA LA PLATAFORMA, PONER TAG "Player" AL PLAYER
    //cuando el player entra a la plataforma, se convierte en hijo de esta y la sigue
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    //cuando el player sale de la plataforma, deja de ser hijo
    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
    
}
