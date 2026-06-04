using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float horizontalInput = 0;
    private float forwardInput = 0;
    private float verticalInput = 0;
    private float verCamAngles = 0;

    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float mouseXSens = 90;
    [SerializeField]
    private float mouseYSens = 90;
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Q)) verticalInput -= 1 * 5 * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E)) verticalInput += 1 * 5 * Time.deltaTime;
        else
        {
            if (verticalInput > -0.05 && verticalInput < 0.05) verticalInput = 0;
            else verticalInput -= 1 * Mathf.Sign(verticalInput) * Time.deltaTime;
        }
        verticalInput = Mathf.Clamp(verticalInput, -1, 1);
        //Debug.Log(verticalInput);

        float mouseX = 0;
        float mouseY = 0;

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouseX = Input.GetAxis("Mouse X") * mouseXSens * Time.deltaTime;
            mouseY = -Input.GetAxis("Mouse Y") * mouseYSens * Time.deltaTime;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        transform.Rotate(Vector3.up, mouseX, Space.World);
        verCamAngles += mouseY;

        verCamAngles = Mathf.Clamp(verCamAngles, -90f, 90f);

        transform.eulerAngles = new Vector3(verCamAngles, transform.eulerAngles.y, transform.eulerAngles.z);

        var moveDir = transform.right * horizontalInput + transform.forward * forwardInput + transform.up * verticalInput;
        moveDir = Vector3.ClampMagnitude(moveDir, 1);

        transform.position += moveDir * speed * Time.deltaTime;
    }
}
