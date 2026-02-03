using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Stats")]
    [SerializeField]
    private float playerMaxHealth;

    [SerializeField]
    private float playerBaseMaxHealth = 10.0f;

    [SerializeField]
    private float playerCurrentHealth;

    [Header("Health Upgrades")]
    [SerializeField]
    private float healthUpgradeDecay = 0.9f;

    [SerializeField]
    public int maxHealthUpgrades = 1;

    private Rigidbody2D rb;

    private void Start()
    {
        RecalculateMaxHealth();
        playerCurrentHealth = playerMaxHealth;
        rb = GetComponent<Rigidbody2D>();
        GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Healthbar>().SetHealth(playerCurrentHealth);
    }

    public void TakeDamage(float amount, Vector3 damageSourcePos, float knockbackModifier)
    {
        if (playerCurrentHealth <= 0) { Die(); return; }

        playerCurrentHealth -= amount;
        GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Healthbar>().SetHealth(playerCurrentHealth);

        Vector3 force = (transform.position - damageSourcePos).normalized * (knockbackModifier * 10000);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(force.x, force.y), ForceMode2D.Impulse);
    }
    public void Heal(float amount)
    {
        //playerCurrentHealth += amount; QUICK FIX TO MAKE THE HEAL UPGRADE TAKE YOU TO FULL HEALTH
        playerCurrentHealth = playerMaxHealth;

        if (playerCurrentHealth > playerMaxHealth) { playerCurrentHealth = playerMaxHealth; }
        GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Healthbar>().SetHealth(playerCurrentHealth);
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject.Find("SceneManagerObj").GetComponent<SceneManagerScript>().LoadDeathScene();
        //some other logic for game over screen or restarting the game
    }

    public void AddMaxHealthUpgrade(int amount)
    {
        maxHealthUpgrades += amount;

        RecalculateMaxHealth();
    }

    private void RecalculateMaxHealth()
    {
        float bonusHealth = 5f * (1f - Mathf.Pow(healthUpgradeDecay, maxHealthUpgrades));
        playerMaxHealth = playerBaseMaxHealth + bonusHealth;

        GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Healthbar>().SetMaxHealth(playerMaxHealth);

        playerCurrentHealth = Mathf.Min(playerCurrentHealth, playerMaxHealth);
    }

    public float GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }
}
