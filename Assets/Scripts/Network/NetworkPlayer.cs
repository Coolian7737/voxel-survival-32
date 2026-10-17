using Mirror;
using UnityEngine;

namespace VoxelPrototype.Networking
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5.5f;
        public float sprintSpeed = 9f;
        public float jumpHeight = 2.5f;
        public float gravity = -18f;

        private CharacterController controller;

        // inputs sent from client to server
        private Vector3 pendingMoveInput;
        private bool pendingJump;
        private bool pendingSprint;

        // server-side velocity for gravity/jumping
        private Vector3 serverVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            // local player keeps a camera; server does authoritative movement
            if (isLocalPlayer)
            {
                Camera.main.transform.SetParent(null);
            }
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                ReadLocalInputAndSend();
            }

            if (isServer)
            {
                ApplyServerMovement();
            }
        }

        private void ReadLocalInputAndSend()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 input = new Vector3(moveX, 0f, moveZ);

            bool jump = Input.GetKeyDown(KeyCode.Space);
            bool sprint = Input.GetKey(KeyCode.LeftShift);

            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0f, mouseX, 0f);

            CmdSendInput(input, jump, sprint, transform.rotation);
        }

        [Command]
        private void CmdSendInput(Vector3 move, bool jump, bool sprint, Quaternion rot)
        {
            // store inputs on server and apply rotation
            pendingMoveInput = move;
            pendingJump = jump;
            pendingSprint = sprint;
            transform.rotation = rot;
        }

        [Server]
        private void ApplyServerMovement()
        {
            if (controller == null) return;

            bool grounded = controller.isGrounded;
            if (grounded && serverVelocity.y < 0f)
                serverVelocity.y = -2f;

            if (pendingJump && grounded)
            {
                serverVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            float activeSpeed = pendingSprint ? sprintSpeed : moveSpeed;
            Vector3 move = new Vector3(pendingMoveInput.x, 0f, pendingMoveInput.z);
            move = transform.TransformDirection(move) * activeSpeed;

            serverVelocity.y += gravity * Time.deltaTime;
            Vector3 total = (move + new Vector3(0f, serverVelocity.y, 0f)) * Time.deltaTime;

            controller.Move(total);
        }
    }
}
