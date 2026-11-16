using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject respawn;
    public TriggerPersecusion persecucion_collider;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        persecucion_collider = GetComponent<TriggerPersecusion>();
        //respawn = GetComponent<>();
    }

    public void Die()
    {
        persecucion_collider.gameObject.SetActive(true);
        Debug.Log("se reactiva el collider");
        rb.linearVelocity = Vector3.zero;
        transform.position = respawn.transform.position;
        SoundFXManager.Instance.Play3DSound("DeathClip", transform.position, volume: 1f);
    }
}
