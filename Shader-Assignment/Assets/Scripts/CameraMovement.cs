using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float verCamAngles = 0;

    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float mouseXSens = 90;
    [SerializeField]
    private float mouseYSens = 90;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        float mouseX = Input.GetAxis("Mouse X") * mouseXSens * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseYSens * Time.deltaTime;

        transform.Rotate(Vector3.up, mouseX, Space.World);
        verCamAngles += mouseY;

        verCamAngles = Mathf.Clamp(verCamAngles, -90f, 90f);

        transform.eulerAngles = new Vector3(verCamAngles, transform.eulerAngles.y, transform.eulerAngles.z);

        transform.position += (transform.right * horizontalInput + transform.forward * verticalInput) * speed * Time.deltaTime;
    }
}
