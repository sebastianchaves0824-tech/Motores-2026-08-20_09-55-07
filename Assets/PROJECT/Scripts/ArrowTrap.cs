using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject flecha;
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public float tiempoDisparo = 3f;
    private float contador = 0f;
    void Start()
    {
        contador = 0f;
    }

  
    void Update()
    {
        contador = contador + Time.deltaTime;
        if (contador >= tiempoDisparo)
        {

            Instantiate(flecha, Point1.position, Point1.rotation);
            
            Instantiate(flecha, Point2.position, Point2.rotation);

            Instantiate(flecha, Point3.position, Point3.rotation);

            contador = 0f;
        }
}
    }
