using System.Collections.Generic;
using Unity.Cinemachine;
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
        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.TryGetComponent<EnemyAI>(out var enemy))
            {
                Debug.Log("Enemy Hit");

                enemy.TakeDamage(
                    (float)playerAttackScript.GetWeaponDamage(),
                    playerAttackScript.transform.position,
                    (float)playerAttackScript.GetWeaponKnockback());
            }
            else
            {
                Debug.LogError("Enemy tagged object mising EnemyAI component: " + collision.gameObject.name);
            }


            enemiesHit.Add(collision.gameObject.GetComponent<EnemyAI>());
        }
    }

    public void ResetHithash()
    {
        enemiesHit.Clear();
    }
}
