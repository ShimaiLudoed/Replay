
    using UnityEngine;

    public class PlayerController3D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private int attackDamage = 10;
        
        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private CommandSource commandSource;
        private Vector3 currentMoveInput;
        
        void Start()
        {
            controller = GetComponent<CharacterController>();
            commandSource = FindObjectOfType<CommandSource>();
            EventBus.Register<MovePayload>("OnMoveCommand", OnMoveCommand);
            EventBus.Register("OnJumpCommand", OnJumpCommand);
            
        }
        
        void Update()
        {
            if (commandSource != null)
            {
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveZ = Input.GetAxisRaw("Vertical");
                
                if (moveX != 0 || moveZ != 0)
                {
                    Debug.Log($"Sending move: x={moveX}, z={moveZ}");
                    commandSource.SendMoveCommand(new Vector3(moveX, 0, moveZ));
                }
                else
                {
                    commandSource.SendMoveCommand(Vector3.zero);
                }
                
                if (Input.GetButtonDown("Jump"))
                {
                    Debug.Log("Sending jump");
                    commandSource.SendJumpCommand();
                }
                
            }
            Vector3 move = transform.TransformDirection(currentMoveInput) * moveSpeed;
            controller.Move(move * Time.deltaTime);
            
            isGrounded = controller.isGrounded;
            
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            
        }
        
        private void OnMoveCommand(MovePayload payload)
        {
            Debug.Log($"Move command: ({payload.x}, {payload.z})");
            currentMoveInput = new Vector3(payload.x, payload.y, payload.z);
        }
        
        private void OnJumpCommand()
        {
            Debug.Log("Jump command received");
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                Debug.Log("Jump executed!");
            }
            else
            {
                Debug.Log("Can't jump - not grounded");
            }
        }
        
    }