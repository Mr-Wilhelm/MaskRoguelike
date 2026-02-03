using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions References")]
    [SerializeField]
    private InputActionReference moveRef;

    [SerializeField]
    private InputActionReference dodgeRef;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [Header("Movement Values")]
    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float baseMoveSpeed = 10.0f;

    [SerializeField]
    private float totalMoveSpeed;

    [SerializeField]
    [Tooltip("The rate at which movement speed is increased." +
        "Higher Value = Slower Rate of Increase")]

    private float moveBonusDecay = 0.9f;

    [SerializeField]
    private float moveSpeedBonus;

    [SerializeField]
    [Tooltip("Percentage cap of movement speed" +
        "E.g. 3.0f = 300%, " +
        "1.7f = 170%")]
    private float moveSpeedCap = 3.0f;

    [Header("Dodging Variables")]
    [SerializeField]
    private float dodgeSpeedModifier = 1.25f;

    [SerializeField]
    private float dodgeDuration = 0.5f;

    [SerializeField]
    private float dodgeCooldown = 0.5f;

    [SerializeField]
    private bool canDodge = true;

    [Header("Player States")]
    [SerializeField]
    private bool isInvulnerable = false;

    [Header("Player Upgrades")]
    [SerializeField]
    public int moveUpgrades = 1;


    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveSpeedMultiplier = 1.0f + (moveSpeedCap - 1.0f) * (1.0f - Mathf.Pow(moveBonusDecay, moveUpgrades));

        moveSpeedBonus = moveSpeedMultiplier;    //diminishing returns maths.

        totalMoveSpeed = moveSpeed * moveSpeedBonus;
        rb.MovePosition(rb.position + moveInput.normalized * totalMoveSpeed * Time.fixedDeltaTime);

        if (moveInput.sqrMagnitude <= 0.01f)
        {
            EnableDodge();
        }

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
    }

    private void OnDisable()
    {
        //basic movement
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= OnMoveCancel;
        moveRef.action.Disable(); //disable to prevent ghost inputs

        //dodging
        dodgeRef.action.started -= OnDodge;
        dodgeRef.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        animator.SetBool("isWalking", true);
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("LastInputX", moveInput.x);
        animator.SetFloat("LastInputY", moveInput.y);
        moveInput = Vector2.zero;
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        if(canDodge)
            animator.SetBool("isDodging", true);
    }

    public void EnableDodge()
    {
        moveSpeed = baseMoveSpeed;
        isInvulnerable = false;

        canDodge = true;
        animator.SetBool("isDodging", false);
    }

    public void DisableDodge()
    {
        canDodge = false;

        moveSpeed = baseMoveSpeed * dodgeSpeedModifier;
        isInvulnerable = true;
        
    }
}
