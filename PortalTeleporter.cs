using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleporter : MonoBehaviour
{
    public Portal_Controller portal;   // assign in Inspector
    public string nextSceneName;       // name of the next level

    private void OnTriggerEnter(Collider other)
    {
        if (!portal) return;

        // Only teleport if portal is powered on
        if (portal.activated && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

