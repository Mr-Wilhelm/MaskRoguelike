using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField]
    private InputActionReference primaryAttackRef;

    [SerializeField]
    private InputActionReference secondaryAttackRef;

    void OnEnable()
    {
        //primary attack
        primaryAttackRef.action.Enable();
        primaryAttackRef.action.started += OnPrimaryAttack;

        //secondary attack
        secondaryAttackRef.action.Enable();
        secondaryAttackRef.action.started += OnSecondaryAttack;
    }

    private void OnDisable()
    {
        //primary attack
        primaryAttackRef.action.started -= OnPrimaryAttack;
        primaryAttackRef.action.Disable();

        //secondary attack
        secondaryAttackRef.action.started -= OnSecondaryAttack;
        secondaryAttackRef.action.Disable();
    }

    private void OnPrimaryAttack(InputAction.CallbackContext context)
    {

    }

    private void OnSecondaryAttack(InputAction.CallbackContext context)
    {

    }
}
