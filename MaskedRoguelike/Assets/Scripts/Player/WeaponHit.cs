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

    private void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        Vector2 mouseDir = mouseWorldPos - (Vector2)pivotPos.position;
        float mouseAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;

        pivotPos.rotation = Quaternion.Euler(0f, 0f, mouseAngle);
    }
}
