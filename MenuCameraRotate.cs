using UnityEngine;

public class MenuCameraRotate : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public float rotateAngle = 30f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        float angle = Mathf.Sin(timer * rotateSpeed * 0.1f) * rotateAngle;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}

