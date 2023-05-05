using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerabaleCounter;
    public bool invulnerabale;

    [Header("事件")]
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Transform> OnDefence;
    public UnityEvent OnDie;

    private void Awake() {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
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
        if (CompareTag("Player") && GetComponent<PlayerController>().isDefence)
            OnDefence?.Invoke(attacker.transform);
        else if (currentHealth - attacker.damgae > 0){
            currentHealth -= attacker.damgae;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else{
            currentHealth = 0;
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
        
    }

    private void TriggerInvulnerable(){
        if (!invulnerabale){
            invulnerabale = true;
            invulnerabaleCounter = invulnerableDuration;
        }
    }
}
