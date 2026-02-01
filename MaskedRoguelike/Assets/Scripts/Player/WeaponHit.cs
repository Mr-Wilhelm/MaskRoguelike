using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHit : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform pivotPos;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private PlayerAttack playerAttackScript;

    private HashSet<EnemyAI> enemiesHit = new HashSet<EnemyAI>();

    private void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        Vector2 mouseDir = mouseWorldPos - (Vector2)pivotPos.position;
        float mouseAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;

        pivotPos.rotation = Quaternion.Euler(0f, 0f, mouseAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
                
        //triple nested if statements im so sorry
        if(collision.gameObject.tag == "Enemy")
        {
            GameObject enemyGameObject = collision.GetComponentInParent<Transform>().parent.gameObject;
            if(enemyGameObject.TryGetComponent<EnemyAI>(out var enemy))
            {
                //Debug.Log("Enemy Hit");

                if(playerAttackScript.isPrimaryAttacking)   //is doing primary attack
                {
                    enemy.TakeDamage(
                        playerAttackScript.GetWeaponDamage(),
                        playerAttackScript.transform.position,
                        EnemyAI.DamageType.Fire,
                        playerAttackScript.GetWeaponKnockback());
                }
                else if(playerAttackScript.isSecondaryAttacking)    //is doing secondary attack
                {
                    enemy.TakeDamage(
                    playerAttackScript.GetWeaponDamage(),
                    playerAttackScript.transform.position,
                    EnemyAI.DamageType.Ice,
                    playerAttackScript.GetWeaponKnockback());
                }
                else //is doing an invalid attack type? (sanity check)
                {
                    enemy.TakeDamage(
                    playerAttackScript.GetWeaponDamage(),
                    playerAttackScript.transform.position,
                    EnemyAI.DamageType.Default,
                    playerAttackScript.GetWeaponKnockback());
                }


            }
            else
            {
                Debug.LogWarning("Enemy tagged object mising EnemyAI component: " + enemyGameObject.name);
            }


            enemiesHit.Add(enemyGameObject.GetComponent<EnemyAI>());
        }
    }

    public void ResetHithash()
    {
        enemiesHit.Clear();
    }
}
