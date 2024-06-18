using UnityEngine;

public class MT : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust the object movement speed
    public float rotationSpeed = 2.0f; // Adjust the camera rotation speed
    public Transform mainCamera;

    void Update()
    {
        // Object movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Camera rotation with mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(-mouseY * rotationSpeed, mouseX * rotationSpeed, 0.0f);
        mainCamera.Rotate(rotation);
    }
}
