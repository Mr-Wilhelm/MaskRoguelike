using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Stats")]
    [SerializeField]
    private float playerMaxHealth;

    [SerializeField]
    private float playerCurrentHealth;

    [Header("Health Upgrades")]
    [SerializeField]
    private float healthUpgradeDecay = 0.9f;

    [SerializeField]
    private int maxHealthUpgrades = 1;

    private void Update()
    {
        //commented out because bugged
        //playerMaxHealth = GetPlayerMaxHealth();
    }

    public void TakeDamage(float amount, Vector3 damageSourcePos, float knockbackModifier)
    {
        if(playerCurrentHealth <= 0) { Die(); return; }

        playerCurrentHealth -= amount;

        Vector3 force = (transform.position - damageSourcePos).normalized * (knockbackModifier * 1000);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x, force.y));
    }
    public void Heal(float amount)
    {
        playerCurrentHealth += amount;

        if (playerCurrentHealth > playerMaxHealth) { playerCurrentHealth = playerMaxHealth; }
    }

    private void Die()
    {
        Destroy(gameObject);
        //some other logic for game over screen or restarting the game
    }

    public float GetPlayerMaxHealth()
    {
        return playerMaxHealth * 1.0f + (1.0f - Mathf.Pow(healthUpgradeDecay, maxHealthUpgrades));
    }
}
