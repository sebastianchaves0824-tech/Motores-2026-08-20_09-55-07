using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private string nombreEscenaMenu = "MainMenu";

    private bool estaPausado = false;

    private void Update()
    {
        // Detecta si se presionó Escape o P usando el nuevo Input System
        if (Keyboard.current != null && 
           (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (estaPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Pausar()
    {
        estaPausado = true;
        panelPausa.SetActive(true);
        Time.timeScale = 0f; // Congela físicas, movimiento y tiempo

        // Libera y muestra el cursor para poder hacer clic en los botones y el slider
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reanudar()
    {
        estaPausado = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Devuelve el tiempo a la normalidad

        // Si en tu juego ocultás el cursor al jugar, podés volver a bloquearlo acá:
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    public void VolverAlMenuPrincipal()
    {
        // Obligatorio restablecer el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}