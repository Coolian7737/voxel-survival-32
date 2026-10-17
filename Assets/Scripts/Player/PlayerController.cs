using UnityEngine;

namespace VoxelPrototype
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5.5f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float jumpHeight = 2.5f;
        [SerializeField] private float gravity = -18f;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float lookSensitivity = 3f;
        [SerializeField] private float maxLookAngle = 60f;

        private CharacterController characterController;
        private Vector3 velocity;
        private float pitch;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (cameraPivot == null)
            {
                var camera = Camera.main;
                if (camera != null)
                {
                    cameraPivot = camera.transform;
                }
            }

            tag = "Player";
        }

        private void Update()
        {
            HandleMovement();
            HandleCamera();
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 input = new Vector3(moveX, 0f, moveZ);
            input = transform.TransformDirection(input);

            bool grounded = characterController.isGrounded;
            if (grounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            float activeSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            Vector3 move = input.normalized * activeSpeed;
            characterController.Move(move * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleCamera()
        {
            if (cameraPivot == null)
            {
                return;
            }

            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            transform.Rotate(0f, mouseX, 0f);
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (Camera.main != null)
            {
                Vector3 desiredPosition = transform.position + new Vector3(0f, 1.6f, 0f) + transform.forward * -4f + Vector3.up * 2.5f;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, Time.deltaTime * 8f);
                Camera.main.transform.LookAt(transform.position + Vector3.up * 1.5f);
            }
        }
    }
}
