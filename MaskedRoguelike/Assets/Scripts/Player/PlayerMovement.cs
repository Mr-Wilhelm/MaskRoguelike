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
    private int moveUpgrades = 1;

    [SerializeField]
    private int healthUpgrades = 1;

    [SerializeField]
    private int damageUpgrades = 1;



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
        float moveSpeedMultiplier = 1.0f + (moveSpeedCap - 1.0f) * (1.0f - Mathf.Pow(moveBonusDecay, moveUpgrades));

        moveSpeedBonus = moveSpeedMultiplier;    //diminishing returns maths.

        totalMoveSpeed = moveSpeed * moveSpeedBonus;
        rb.MovePosition(rb.position + moveInput.normalized * totalMoveSpeed * Time.fixedDeltaTime);
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
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        if(canDodge) { StartCoroutine(Dodge()); }
    }

    private IEnumerator Dodge()
    {
        canDodge = false;
        float priorDodgeSpeed = moveSpeed;
        moveSpeed = (baseMoveSpeed * dodgeSpeedModifier);
        isInvulnerable = true;
        yield return new WaitForSeconds(dodgeDuration);

        moveSpeed = priorDodgeSpeed;
        isInvulnerable = false;
        yield return new WaitForSeconds(dodgeCooldown);

        canDodge = true;
    }
}
