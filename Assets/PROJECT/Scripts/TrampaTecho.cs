using UnityEngine;

public class TrampaTecho : MonoBehaviour
{
    [SerializeField] private float alturaHardcodeada;
    private Rigidbody rbTrampa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbTrampa = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("se detecta el trigger");
        rbTrampa.useGravity = true;
    }
}
