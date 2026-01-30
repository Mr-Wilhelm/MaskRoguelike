using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveRef;

    [SerializeField]
    private InputActionReference dodgeRef;

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private 

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
        //basic movement
        moveRef.action.Enable();
        moveRef.action.performed += OnMove;
        moveRef.action.canceled += OnMoveCancel;

        //dodging
        dodgeRef.action.Enable();
        dodgeRef.action.started += OnDodge;
        dodgeRef.action.canceled += OnDodgeCancel;
    }

    private void OnDisable()
    {
        //basic movement
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= OnMoveCancel;
        moveRef.action.Disable(); //disable to prevent ghost inputs

        //dodging
        dodgeRef.action.started -= OnDodge;
        dodgeRef.action.canceled -= OnDodgeCancel;
        dodgeRef.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        Debug.Log("Dodging");
    }

    private void OnDodgeCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Stop Dodging");
    }
}
