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

    public enum AttackType
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

    public bool isPrimaryAttacking = false;

    public bool isSecondaryAttacking = false;

    [Header("Weapon Status Effects")]

    public float burnDamageOverTime = 0.1f;

    public float burnDuration = 1.0f;

    public float frostMovementSpeedReduction = 1.0f;

    public float frostDuration = 1.0f;

    [Header("Weapon Upgrades")]

    public int damageUpgrades = 0;

    public int knockbackUpgrades = 0;

    public int burnDamageUpgrades = 0;

    public int burnDurationUpgrades = 0;

    public int frostSlowUpgrades = 0;

    public int frostDurationUpgrades = 0;

    public float weaponUpgradeDecay = 0.9f;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

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
                isPrimaryAttacking = true;
                isSecondaryAttacking = false;
                animator.SetBool("isFireAttack", true);
                animator.SetBool("isIceAttack", false);
                break;
            case AttackType.secondary:
                isPrimaryAttacking = false;
                isSecondaryAttacking = true;
                animator.SetBool("isFireAttack", false);
                animator.SetBool("isIceAttack", true);
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
        animator.SetBool("isFireAttack", false);
        animator.SetBool("isIceAttack", false);
        canAttack = true;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, damageUpgrades));
    }

    public float GetWeaponKnockback()
    {
        return weaponKnockbackStrength * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, knockbackUpgrades));
    }

    public float GetBurnDamageOverTime()
    {
        return burnDamageOverTime * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, burnDamageUpgrades));
    }

    public float GetBurnDuration()
    {
        return burnDuration * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, burnDurationUpgrades));
    }
    public float GetFrostReductionAmount()
    {
        return frostMovementSpeedReduction * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, frostSlowUpgrades));
    }

    public float GetFrostSlowDuration()
    {
        return frostDuration * 1.0f + (1.0f - Mathf.Pow(weaponUpgradeDecay, frostDurationUpgrades));
    }

}