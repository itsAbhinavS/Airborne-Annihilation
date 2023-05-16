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
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    /*void LateUpdate()
    {
        // Camera movement
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        yRotation += mouseX;
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Calculate camera offset based on player rotation
        Vector3 cameraOffset = rotation * new Vector3(2f, 0f, 0f); // Offset in the x-axis

        // Camera position
        Vector3 cameraPosition = playerTransform.position - cameraTransform.forward * cameraDistance + cameraOffset;
        cameraPosition += Vector3.up * Mathf.Sin(breathOffset) * breathIntensity;
        cameraTransform.position = cameraPosition;
        cameraTransform.rotation = rotation;
        breathOffset += breathSpeed * Time.deltaTime;
    }*/


    void LateUpdate()
    {
        // Camera movement
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        yRotation += mouseX;

        // Smooth camera rotation
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, rotation, Time.deltaTime * 30);

        // Calculate camera offset based on player rotation
        Vector3 cameraOffset = rotation * new Vector3(2f, 0f, 0f); // Offset in the x-axis

        // Camera position
        Vector3 targetPosition = playerTransform.position - cameraTransform.forward * cameraDistance + cameraOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref velocity, 0.01f);
        cameraTransform.position = smoothedPosition;

        breathOffset += breathSpeed * Time.deltaTime;
    }

}
