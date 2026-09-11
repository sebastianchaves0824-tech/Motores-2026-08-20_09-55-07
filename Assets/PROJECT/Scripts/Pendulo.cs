using UnityEngine;

public class Pendulo : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit=75f;
    public bool randomStart = false; 
    private float random=0;
    void Awake()
    {
        if (randomStart )
            random = Random.Range(0f, 1f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float angle = limit * Mathf.Sin( Time.time + random * speed );
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
