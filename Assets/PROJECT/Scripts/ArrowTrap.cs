using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject flecha;
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public float tiempoDisparo = 3f;
    [SerializeField] private float tiempoDeVidaFlechas = 4f; // Tiempo antes de eliminar la flecha
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
            // Instanciar y guardar la referencia de cada proyectil
            GameObject flecha1 = Instantiate(flecha, Point1.position, Point1.rotation);
            GameObject flecha2 = Instantiate(flecha, Point2.position, Point2.rotation);
            GameObject flecha3 = Instantiate(flecha, Point3.position, Point3.rotation);

            // Programar su destrucción automática tras transcurrir tiempoDeVidaFlechas
            Destroy(flecha1, tiempoDeVidaFlechas);
            Destroy(flecha2, tiempoDeVidaFlechas);
            Destroy(flecha3, tiempoDeVidaFlechas);

            contador = 0f;
        }
    }
}