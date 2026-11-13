using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
   private void OnTriggerEnter(Collider other) 
    { 
      if(other.tag == "Exit") 
        {
            SceneManager.LoadScene("Persecucion");
        }
    }
}
