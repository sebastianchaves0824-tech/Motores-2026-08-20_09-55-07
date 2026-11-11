using UnityEngine;

public class ColliderIgnorer : MonoBehaviour
{
    
    public Collider playerCollider;

  
    public Collider targetBoxCollider;

    void Start()
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider>();
        }

        if (playerCollider != null && targetBoxCollider != null)
        {
            
            Physics.IgnoreCollision(playerCollider, targetBoxCollider, true);
        }
    }
}
