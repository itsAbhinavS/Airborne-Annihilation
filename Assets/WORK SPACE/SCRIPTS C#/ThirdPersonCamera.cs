using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraSensitivity = 3f;
    [SerializeField] private float breathSpeed = 2f;
    [SerializeField] private float breathIntensity = 0.1f;
    [SerializeField] private float minVerticalAngle = -15f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private Transform cameraTransform;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float breathOffset = 0f;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    void LateUpdate()
    {
        // Camera movement
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        yRotation += mouseX;
        cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Camera position
        Vector3 cameraPosition = playerTransform.position - cameraTransform.forward * cameraDistance;
        cameraPosition += Vector3.up * Mathf.Sin(breathOffset) * breathIntensity;
        cameraTransform.position = cameraPosition;
        breathOffset += breathSpeed * Time.deltaTime;
    }
}
