using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.Instance.healthSlider.maxValue = maxHealth;
        UIController.Instance.healthSlider.value = currentHealth;

        UIController.Instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void damagePlayer(int damage)
    {
        currentHealth -= damage;

        UIController.Instance.showDamage();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            currentHealth = 0;
            GameManager.Instance.playerDied();
        }
        UIController.Instance.healthSlider.value = currentHealth;

        UIController.Instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.Instance.healthSlider.value = currentHealth;

        UIController.Instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }
}

