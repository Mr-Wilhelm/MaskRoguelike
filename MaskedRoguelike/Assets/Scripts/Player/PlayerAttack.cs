using System.Collections;
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

    private enum AttackType
    {
        Default,
        primary,
        secondary
    }

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
        Attack(AttackType.primary);
    }

    private void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        Attack(AttackType.secondary);
    }

    private void Attack(AttackType attackType)
    {
        if(attackType != AttackType.primary && attackType!= AttackType.secondary)   //sanity check against what enum is input
        {
            Debug.LogWarning("Invalid Attack Type Given");
            return;
        }

        switch (attackType)
        {
            case AttackType.primary:
                Debug.Log("Primary Attack");
                break;
            case AttackType.secondary:
                Debug.Log("Secondary Attack");
                break;
        }
    }
}
