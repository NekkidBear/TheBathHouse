// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private float verticalVelocity = 0f;
    private MiniMap miniMap;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        miniMap = FindObjectOfType<MiniMap>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJumping();

        // Toggle mini-map with 'M' key if enabled
        if (Input.GetKeyDown(KeyCode.M) && miniMap != null && miniMap.isEnabled)
        {
            miniMap.gameObject.SetActive(!miniMap.gameObject.activeSelf);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") + Input.GetAxisRaw("HorizontalArrow");
        float moveVertical = Input.GetAxis("Vertical") + Input.GetAxisRaw("VerticalArrow");

        // Clamp values to ensure diagonal movement isn't faster
        moveHorizontal = Mathf.Clamp(moveHorizontal, -1f, 1f);
        moveVertical = Mathf.Clamp(moveVertical, -1f, 1f);

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    void HandleJumping()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 verticalMovement = transform.up * verticalVelocity * Time.deltaTime;
        controller.Move(verticalMovement);
    }
}