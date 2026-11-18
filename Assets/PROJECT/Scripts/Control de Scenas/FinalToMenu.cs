using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalToMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entro al collider");
        
        SceneManager.LoadScene("MainMenu");
       
    }
}