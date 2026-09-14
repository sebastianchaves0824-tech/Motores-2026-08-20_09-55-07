using UnityEngine;

public class IceTile : MonoBehaviour
{
    [Header("Ajuste de Hielo")]
    [Tooltip("Valor de frenado sobre hielo (mas bajo = patina mucho mas)")]
    [SerializeField] private float deceleracionHielo = 2f;

    [Tooltip("Valor de frenado normal del personaje")]
    [SerializeField] private float deceleracionNormal = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetDeceleration(deceleracionHielo);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Le avisa al jugador que salió del hielo para que restaure cuando toque suelo firme
                player.SalirDeSuperficieHielo(deceleracionNormal);
            }
        }
    }
}