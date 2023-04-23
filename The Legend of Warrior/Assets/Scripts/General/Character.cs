using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;

    public float invulnerableDuration;
    private float invulnerabaleCounter;
    public bool invulnerabale;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void Update() {
        if (invulnerabale){
            invulnerabaleCounter -= Time.deltaTime;
            if (invulnerabaleCounter <= 0){
                invulnerabale = false;
            }
        }
    }

    public void TakeDamage(Attack attacker){
        if (invulnerabale)
            return;
        if (currentHealth - attacker.damgae > 0){
            currentHealth -= attacker.damgae;
            TriggerInvulnerable();
        }
        else{
            currentHealth = 0;
        }
        
    }

    private void TriggerInvulnerable(){
        if (!invulnerabale){
            invulnerabale = true;
            invulnerabaleCounter = invulnerableDuration;
        }
    }
}
