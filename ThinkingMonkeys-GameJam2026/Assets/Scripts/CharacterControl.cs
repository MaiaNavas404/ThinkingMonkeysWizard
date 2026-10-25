using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _playerSpeed = 5;
    //[SerializeField]
    //private float _acceleration = 2.5f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private float _verticalVelocity;
    private bool _isCrouching = false;

    [SerializeField]
    private float _gravity = -6f;
    private float _jumpHeight = 2f;
    [SerializeField]
    
    private bool _isJumping = false;

    //Camera stuff
    [SerializeField]
    private Transform mainCamera;


    void Update()
    {
        InputAction jumpAction = InputSystem.actions.FindAction("Jump");
        if (jumpAction.WasPressedThisFrame()) //characterController.isGrounded
        {
            _isJumping = true;
            Jump();
        }
        HandleCrouch();
        //This shi dont fucking work :sob: Try the traditional way again now that applying gravity works
        //if (_isJumping)
        //{
        //    _elapsedTime += Time.deltaTime;
        //    _gravity *= -1;
        //    if (_elapsedTime > 0.5f)
        //    {
        //        _gravity *= -1;
        //        _isJumping = false;
        //        _elapsedTime = 0f;
        //    }
        //}
        HandleMovement();
        ApplyGravity();

    }

    private void IsGroundedCheck()
    {
        //Improved version of _isGrounded since the built in unity one seems incredibly inconsistent in my project
        //if (Physics.SphereCast(characterController.center, 1, new Vector3(0, -1, 0) characterController.height / 2))
    }

    private void Jump()
    {
        _verticalVelocity = Mathf.Sqrt(_jumpHeight*-_jumpHeight*_gravity);
        _isJumping = false;
    }

    private void HandleMovement()
    {
        //Get input axis
        InputAction moveAction = InputSystem.actions.FindAction("Move");
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        //Refine Movement
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        if (direction != Vector3.zero)
        {
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            switch (_isCrouching)
            {
                case true:
                    characterController.Move(0.5f * (moveDirection.normalized * _playerSpeed * Time.deltaTime));
                    break;
                case false:
                    characterController.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
                    break;
            }
            //Character rotation
            //Another case statement that will execute if Z Targeting is false
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    private void ApplyGravity()
    {
        //if (!characterController.isGrounded)
        //{
        //if (_verticalVelocity < 0)
           // _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;
        characterController.Move(new Vector3(0,_verticalVelocity, 0));
        //}
    }
    private void HandleCrouch()
    {
        InputAction crouchAction = InputSystem.actions.FindAction("Crouch");
        //Previously required character to be grounded but isgrounded seems to not work majority of the time and cause jittering with my character.
        if (crouchAction.IsPressed()) _isCrouching = true;
        else _isCrouching = false;
        
        
        switch (_isCrouching)
        {
            case true:
                //Because character shrinks from origin which is at the middle, character is not grounded which causes it to jitter
                _player.transform.localScale = new Vector3(1, 0.5f, 1);
                //if (crouchAction.WasPressedThisFrame())
                //_player.transform.position += new Vector3(0, -0.6f, 0);
                break;
            case false:
                _player.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
        //Forcibly moves character down half a unit when crouching for the first time so capsule aligns with ground again
                if (crouchAction.WasPressedThisFrame())
                    characterController.Move(new Vector3(0, -0.5f, 0));
    }
}

