using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    public DeathScreenManager deathManager;

    [Header("Movement")]
    public float speed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Camera Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 200f;

    [Header("Step Shake")]
    public float stepFrequency = 7f;
    public float stepAmplitude = 0.03f;
    public float stepSmooth = 10f;

    [Header("Camera Blur")]
    public MotionBlur motionBlur;
    public float blurStrength = 0.6f;
    public float blurSmooth = 8f;

    [Header("Respawn")]
    public Vector3 spawnPoint = new Vector3(-27.54202f, 1.5f, -31.43429f);

    // ⭐ NEW: Bounce variable for trampolines
    public float bounceVelocity = 0f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private float stepTimer = 0f;
    private Vector3 camStartPos;
    private float currentBlur = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camStartPos = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        LookAround();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount = new Vector2(h, v).magnitude;

        StepShake(moveAmount);
    }

    void MovePlayer()
    {
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -10f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // ⭐ NEW: Bounce overrides jump
        if (bounceVelocity > 0f)
        {
            velocity.y = bounceVelocity;
            bounceVelocity = 0f;
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        float turnAmount = Mathf.Abs(mouseX) + Mathf.Abs(mouseY);
        currentBlur = Mathf.Lerp(currentBlur, turnAmount * blurStrength, Time.deltaTime * blurSmooth);

        if (motionBlur != null)
            motionBlur.shutterAngle.Override(currentBlur * 180f);
    }

    void StepShake(float moveAmount)
    {
        if (!controller.isGrounded || moveAmount < 0.1f)
        {
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                camStartPos,
                Time.deltaTime * stepSmooth
            );
            return;
        }

        stepTimer += Time.deltaTime * stepFrequency;
        float shake = Mathf.Sin(stepTimer) * stepAmplitude;

        Vector3 target = camStartPos + new Vector3(0, shake, 0);

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            target,
            Time.deltaTime * stepSmooth
        );
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Lava"))
        {
            deathManager.ShowDeathScreen("You fell into lava");
        }
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = spawnPoint;
        velocity = Vector3.zero;
        controller.enabled = true;
    }
}

