using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControlScript : MonoBehaviour
{
    //[SerializeField]
    //private float _speed = 5f;
    //[SerializeField]
    //private float _rotationSpeed = 30f;
    //[SerializeField]
    //private Transform _cam;
    //[SerializeField]
    //private Vector2 _yLimit = new Vector2(-20f, 20f);

    //private float _currentYAngle = 0f;



    //NEW -----------------------------------------------------------

    //[SerializeField]
    //private CharacterController characterController;
    //[SerializeField]
    //private GameObject _player;
    //[SerializeField]
    //private float _playerSpeed = 5;
    //[SerializeField]
    //private float turnSmoothTime = 0.1f;
    //private float turnSmoothVelocity;

    //private float _verticalVelocity;

    //[SerializeField]
    //private float _gravity = -6f;
    //private float _jumpHeight = 2f;
    //[SerializeField]

    //private bool _isJumping = false;

    //private bool _isCrouching = false;


    ////Camera stuff
    //[SerializeField]
    //private Transform mainCamera;



    //NEW 2222222222222-----------------------------------------------------------

    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //// keyboard
        //InputAction moveAction = InputSystem.actions.FindAction("Move");

        //Vector2 axesVector = moveAction.ReadValue<Vector2>();
        //Vector3 moveVector = transform.right * axesVector.x + transform.forward * axesVector.y;

        //transform.position += moveVector * _speed * Time.deltaTime;

        //// mouse
        //InputAction lookAction = InputSystem.actions.FindAction("Look");

        //float mouseX = lookAction.ReadValue<Vector2>().x * _rotationSpeed * Time.deltaTime;
        //float mouseY = lookAction.ReadValue<Vector2>().y * _rotationSpeed * Time.deltaTime;

        ////horizontal rotation (cyborg)
        //transform.Rotate(0, mouseX, 0);

        ////vertical rotation (camera)
        //_currentYAngle = Mathf.Clamp(_currentYAngle - mouseY, _yLimit.x, _yLimit.y);
        //_cam.localEulerAngles = new Vector3(_currentYAngle, 0, 0);





        //NEWWWW -----------------------------------------------------------

        //InputAction jumpAction = InputSystem.actions.FindAction("Jump");
        //if (jumpAction.WasPressedThisFrame()) //characterController.isGrounded
        //{
        //    _isJumping = true;
        //    Jump();
        //}

        //HandleMovement();
        //ApplyGravity();

        //NEWWWW 222222222222-----------------------------------------------------------

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
            transform.forward = move;

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

    }

    //private void Jump()
    //{
    //    _verticalVelocity = Mathf.Sqrt(_jumpHeight * -_jumpHeight * _gravity);
    //    _isJumping = false;
    //}
    //private void HandleMovement()
    //{
    //    //Get input axis
    //    InputAction moveAction = InputSystem.actions.FindAction("Move");
    //    Vector2 moveInput = moveAction.ReadValue<Vector2>();
    //    Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    //    //Refine Movement
    //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
    //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //    if (direction != Vector3.zero)
    //    {
    //        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //        switch (_isCrouching)
    //        {
    //            case true:
    //                characterController.Move(0.5f * (moveDirection.normalized * _playerSpeed * Time.deltaTime));
    //                break;
    //            case false:
    //                characterController.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
    //                break;
    //        }
    //        //Character rotation
    //        //Another case statement that will execute if Z Targeting is false
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    //    }
    //}
    //private void ApplyGravity()
    //{
    //    //if (!characterController.isGrounded)
    //    //{
    //    //if (_verticalVelocity < 0)
    //    // _verticalVelocity = -2f;

    //    _verticalVelocity += _gravity * Time.deltaTime;
    //    characterController.Move(new Vector3(0, _verticalVelocity, 0));
    //    //}
    //}

    //AAAAAAAAAAAHHHHHHHHHHHHHH-----------------------------------------------------





}
