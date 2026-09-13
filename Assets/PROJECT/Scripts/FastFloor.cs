using UnityEngine;

public class FastFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        player.maxSpeed = 30;
        player.acceleration = 20;
        player.deceleration = 10;
        player.jumpForce = 6;
    }
}
