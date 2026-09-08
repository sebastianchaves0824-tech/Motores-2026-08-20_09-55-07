using UnityEngine;

public class IceTile : MonoBehaviour
{
    [Header("Ajuste de Hielo")]
    [Tooltip("Valor de frenado sobre hielo (mas bajo = patina mucho mas)")]
    [SerializeField] private float deceleracionHielo = -2f;

    [Tooltip("Valor de frenado normal del personaje cuando no esta en hielo")]
    [SerializeField] private float deceleracionNormal = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")  )
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.SetDeceleration(deceleracionHielo);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.SetDeceleration(deceleracionNormal);
            }
        }
    }
}