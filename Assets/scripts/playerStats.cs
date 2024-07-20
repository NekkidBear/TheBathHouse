// PlayerStats.cs
using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float baseDamage = 10f;
    public float currentDamage;
    public float damageModifier = 1f; // New
    public float baseSpeed = 5f;
    public float currentSpeed;
    public float speedModifier = 1f; // New
    public float healthRegenRate = 2f; // New

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;
        currentSpeed = baseSpeed;
        StartCoroutine(HealthRegenRoutine()); // New
    }

    IEnumerator HealthRegenRoutine() // New
    {
        while (true)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += healthRegenRate * Time.deltaTime;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
            }
            yield return null;
        }
    }

    public void IncreaseHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

    public void IncreaseDamage(float amount)
    {
        baseDamage += amount; // Update to modify baseDamage instead
        UpdateDamage(); // New
    }

    public void IncreaseSpeed(float amount)
    {
        baseSpeed += amount; // Update to modify baseSpeed instead
        UpdateSpeed(); // New
    }

    public void ApplyDamageModifier(float modifier, float duration) // New
    {
        damageModifier = modifier;
        UpdateDamage();
        StartCoroutine(ResetModifierAfterDuration(() => { damageModifier = 1f; UpdateDamage(); }, duration));
    }

    public void ApplySpeedModifier(float modifier, float duration) // New
    {
        speedModifier = modifier;
        UpdateSpeed();
        StartCoroutine(ResetModifierAfterDuration(() => { speedModifier = 1f; UpdateSpeed(); }, duration));
    }

    IEnumerator ResetModifierAfterDuration(System.Action resetAction, float duration) // New
    {
        yield return new WaitForSeconds(duration);
        resetAction();
    }

    void UpdateDamage() // New
    {
        currentDamage = baseDamage * damageModifier;
    }

    void UpdateSpeed() // New
    {
        currentSpeed = baseSpeed * speedModifier;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
    }
}

