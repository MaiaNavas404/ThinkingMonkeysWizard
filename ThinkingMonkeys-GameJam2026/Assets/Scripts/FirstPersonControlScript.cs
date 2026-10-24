using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControlScript : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotationSpeed = 30f;
    [SerializeField]
    private Transform _cam;
    [SerializeField]
    private Vector2 _yLimit = new Vector2(-20f, 20f);

    private float _currentYAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard
        InputAction moveAction = InputSystem.actions.FindAction("Move");

        Vector2 axesVector = moveAction.ReadValue<Vector2>();
        Vector3 moveVector = transform.right * axesVector.x + transform.forward * axesVector.y;

        transform.position += moveVector * _speed * Time.deltaTime;

        // mouse
        InputAction lookAction = InputSystem.actions.FindAction("Look");

        float mouseX = lookAction.ReadValue<Vector2>().x * _rotationSpeed * Time.deltaTime;
        float mouseY = lookAction.ReadValue<Vector2>().y * _rotationSpeed * Time.deltaTime;

        //horizontal rotation (cyborg)
        transform.Rotate(0, mouseX, 0);

        //vertical rotation (camera)
        _currentYAngle = Mathf.Clamp(_currentYAngle - mouseY, _yLimit.x, _yLimit.y);
        _cam.localEulerAngles = new Vector3(_currentYAngle, 0, 0);
    }
}
