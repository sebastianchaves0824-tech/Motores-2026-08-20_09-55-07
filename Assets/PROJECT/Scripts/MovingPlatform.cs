using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    public GameObject position1;
    public GameObject position2;
    private bool movingTo1;
    
    void Update()
    {
        MovePlatform();
    }
    void MovePlatform()
    {
        // si esta lejos del punto 1 y se esta moviendo al punto 1 entonces usamos MoveTowards para que vaya para alla
        if (Vector3.Distance(transform.position, position1.transform.position) >= 0.5f && movingTo1)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1.transform.position, platformSpeed*Time.deltaTime);
        }
        // si no se está moviendo al punto 1 entonces va al 2
        else if(Vector3.Distance(transform.position, position2.transform.position) >= 0.5f)
        {
            movingTo1 = false;
            transform.position = Vector3.MoveTowards(transform.position, position2.transform.position, platformSpeed *Time.deltaTime);
        }
        // si no va para la 2 entonces hacemos verdader la condicion para que vuelva a la 1
        else { movingTo1 = true; }
    }
    // aca si colisiona, hago hijo al jugador para que siga automaticamente a la plataforma
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(transform);
    }
    //aca si nos movemos o saltamos va a dejar de ser hijo y nos vamos a mover libremente
    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.SetParent(null);
    }
}
