using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference actionRef;

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

    }

    void OnEnable()
    {
        actionRef.action.Enable();
        actionRef.action.performed += OnMove;
        actionRef.action.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        actionRef.action.performed -= OnMove;
        actionRef.action.canceled -= OnMoveCanceled;
        actionRef.action.Disable(); //disable to prevent ghost inputs
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
}
