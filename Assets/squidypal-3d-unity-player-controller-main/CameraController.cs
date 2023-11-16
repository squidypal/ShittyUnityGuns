using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 100.0f;
    private float cameraRotationX = 0.0f;

    void Update()
    {
        if (player == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the vertical rotation of the camera.
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}