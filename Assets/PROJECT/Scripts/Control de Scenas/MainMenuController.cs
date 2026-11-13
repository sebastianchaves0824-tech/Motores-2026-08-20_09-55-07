using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Escena del Nivel")]
    [SerializeField] private string nombreEscenaJuego = "Level_1";

    private void Start()
    {
        // Restablece el tiempo y libera el cursor por si se vuelve desde Game Over o Pausa
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Permite detener la ejecución también mientras probás dentro del Editor de Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}