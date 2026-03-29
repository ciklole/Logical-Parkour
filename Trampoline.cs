using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var movement = other.GetComponent<PlayerMovement>(); // your movement script name

            if (movement != null)
            {
                movement.bounceVelocity = bounceForce;
            }
        }
    }
}

