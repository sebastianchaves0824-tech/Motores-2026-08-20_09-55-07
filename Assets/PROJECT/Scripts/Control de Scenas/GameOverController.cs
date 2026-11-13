using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("Nombres de Escenas")]
    [SerializeField] private string nombreEscenaNivel = "Level_1";
    [SerializeField] private string nombreEscenaMenu = "MainMenu";

    private void Start()
    {
        // Asegura que el tiempo corra normal y el cursor esté libre para hacer clic
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReintentarNivel()
    {
        SceneManager.LoadScene(nombreEscenaNivel);
    }

    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}