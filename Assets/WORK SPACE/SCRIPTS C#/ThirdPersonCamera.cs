using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Calculate the camera's rotation.
        Vector3 targetPosition = target.position;
        Vector3 cameraPosition = transform.position;
        Vector3 deltaPosition = targetPosition - cameraPosition;
        Quaternion rotation = Quaternion.LookRotation(deltaPosition);

        // Rotate the camera.
        transform.rotation = rotation * transform.rotation;

        // Move the camera towards the target.
        Vector3 offset = transform.forward * Time.deltaTime * distance;
        transform.position += offset;
    }

    void LateUpdate()
    {
        // Handle the camera rotation when the mouse is moved.
        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(x * rotationSpeed * Time.deltaTime, y * rotationSpeed * Time.deltaTime, 0);
        }
    }

}
