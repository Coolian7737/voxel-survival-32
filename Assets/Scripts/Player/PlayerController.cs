using UnityEngine;

namespace VoxelPrototype
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float sprintSpeed = 12f;
        [SerializeField] private float jumpHeight = 2.5f;
        [SerializeField] private float gravity = -20f;

        private CharacterController characterController;
        private Vector3 velocity;
        private bool isGrounded;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 input = new Vector3(moveX, 0f, moveZ);
            input = transform.TransformDirection(input);

            isGrounded = characterController.isGrounded;
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            float activeSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            Vector3 move = input.normalized * activeSpeed;
            characterController.Move(move * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            if (Camera.main != null)
            {
                Camera.main.transform.position = Vector3.Lerp(
                    Camera.main.transform.position,
                    transform.position + new Vector3(0f, 6f, -8f),
                    Time.deltaTime * 5f);

                Camera.main.transform.LookAt(transform.position + Vector3.up * 2f);
            }
        }
    }
}
