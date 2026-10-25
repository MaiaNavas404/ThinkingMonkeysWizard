using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LastCharacterControllerTrust : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minLookAngle = -80f;
    [SerializeField] private float maxLookAngle = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Dash UI")]
    [SerializeField] private Slider dashCooldownSlider;

    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;
    private bool isDashing;
    [SerializeField] private AudioClip _dashSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        controller = GetComponent<CharacterController>();

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (dashCooldownSlider != null)
        {
            dashCooldownSlider.minValue = 0f;
            dashCooldownSlider.maxValue = 1f;
            dashCooldownSlider.value = 1f;
        }
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        if (playerCamera == null)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player horizontally.
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically.
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(
            cameraPitch, minLookAngle, maxLookAngle
        );

        playerCamera.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }


    private void Move()
    {
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move =
            transform.right * horizontal +
            transform.forward * vertical;

        move = Vector3.ClampMagnitude(move, 1f);

        // Update dash cooldown.
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (dashCooldownSlider != null)
        {
            dashCooldownSlider.value =
                1f - Mathf.Clamp01(dashCooldownTimer / dashCooldown);
        }

        // Start a dash when Shift is pressed and movement is active.
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            !isDashing &&
            dashCooldownTimer <= 0f &&
            move.sqrMagnitude > 0.01f)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashDirection = move.normalized;

            _audioSource.PlayOneShot(_dashSound);
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            // Smoothly slow the dash down over its duration.
            float progress = 1f - Mathf.Clamp01(dashTimer / dashDuration);
            float currentDashSpeed = dashSpeed * (1f - progress * progress);

            controller.Move(
                dashDirection * currentDashSpeed * Time.deltaTime
            );

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        // Jump with Space.
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity normally during and outside the dash.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
