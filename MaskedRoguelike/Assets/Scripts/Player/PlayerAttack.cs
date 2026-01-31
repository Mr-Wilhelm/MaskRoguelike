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

    [Header("Hit Radius Variables")]
    [SerializeField]
    private GameObject weaponHitArea;

    [Header("Weapon Attack Variables")]

    [SerializeField]
    private WeaponHit weapon;

    [SerializeField]
    private bool canAttack = true;

    [SerializeField]
    private float weaponAttackDuration = 0.25f;

    [SerializeField]
    private float weaponAttackCooldown = 0.25f;

    [SerializeField]
    private float weaponDamage = 10.0f;

    [SerializeField]
    private float weaponKnockbackStrength = 1.0f;

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

        if (!canAttack) { return; }

        weaponHitArea.GetComponent<PolygonCollider2D>().enabled = true;

        switch (attackType)
        {
            case AttackType.primary:
                Debug.Log("Primary Attack");
                break;
            case AttackType.secondary:
                Debug.Log("Secondary Attack");
                break;
        }

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(weaponAttackDuration);
        weaponHitArea.GetComponent<PolygonCollider2D>().enabled = false;
        weapon.ResetHithash();  //reset the hash of enemies hit
        yield return new WaitForSeconds(weaponAttackCooldown);
        canAttack = true;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public float GetWeaponKnockback()
    {
        return weaponKnockbackStrength;
    }

}