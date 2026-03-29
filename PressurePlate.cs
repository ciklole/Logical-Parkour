using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Portal_Controller portal;   // assign in Inspector
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // Activate the portal
            portal.TogglePortal(true);

            // Optional: lower the plate visually
            transform.localPosition -= new Vector3(0, 0.1f, 0);
        }
    }
}
