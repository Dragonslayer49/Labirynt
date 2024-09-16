using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public PauseMenu pauseMenu;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * pauseMenu.mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * pauseMenu.mouseSensitivity * Time.fixedDeltaTime;

        player.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
