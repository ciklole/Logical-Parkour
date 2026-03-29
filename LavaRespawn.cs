using UnityEngine;

public class LavaRespawn : MonoBehaviour
{
    public Vector3 spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            // Disable controller before teleporting (important)
            controller.enabled = false;

            other.transform.position = spawnPoint;

            // Re-enable controller
            controller.enabled = true;
        }
    }
}

