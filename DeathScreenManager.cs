using UnityEngine;
using TMPro;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreen;
    public TextMeshProUGUI deathText;
    public PlayerMovement player;

    public void ShowDeathScreen(string cause)
    {
        deathText.text = cause;
        deathScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        deathScreen.SetActive(false);
        Time.timeScale = 1f;
        player.Respawn();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

