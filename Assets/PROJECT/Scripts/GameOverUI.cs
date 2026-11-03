using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Panel Principal")]
    [SerializeField] private GameObject panelGameOver;

    public void MostrarPantallaDerrota()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            // Opcional: desbloquear y hacer visible el cursor si estaba oculto en 3ra persona
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReintentarNivel()
    {
        // Recarga la escena actual completa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}