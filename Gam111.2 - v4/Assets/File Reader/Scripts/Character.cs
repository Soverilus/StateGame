using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour {

    public CharacterStats stats = new CharacterStats();
    public int trueHealth;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool wasDead = false;
    public int temporaryHealth;
    Character newTarget;
    private bool isAttacking = false;
    public int totalHealth;
    Animator animator;
    Light spotlight;
    public int level = 1;
    [HideInInspector]
    public int skillUse = 0;
    [SerializeField]
    SkinnedMeshRenderer[] meshEnable;
    GameObject player;
    Character playerChar;

    void Start() {
        trueHealth = stats.maxHealth;
        animator = GetComponent<Animator>();

        if (GetComponentInChildren<Light>() != null) {
            spotlight = GetComponentInChildren<Light>();
            spotlight.enabled = false;
        }
    }
    public void StatMult() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerChar = player.GetComponent<Character>();
        stats.maxHealth *= level;
        stats.energy *= level;
        stats.power *= level;
        for (int i = 1; i <= 4; i++) {
            if (i - level == 0) {
                meshEnable[i - 1].enabled = true;
            }
        }
    }

    void Update() {
        totalHealth = trueHealth + temporaryHealth;
        if (trueHealth <= 0 && isDead == false) {
            Death();
        }
        if (animator.GetAnimatorTransitionInfo(0).IsName("Attack -> Idle") && isAttacking) {
            newTarget.Damage(stats.power);
            isAttacking = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            spotlight.enabled = false;
        }
    }

    public void Damage(int damage) {
        int newDamage = damage - temporaryHealth;
        temporaryHealth -= damage;
        if (temporaryHealth < 0) {
            temporaryHealth = 0;
        }
        //damages the character then sets health to 0 if under 0, or to maxHealth if it's above maxHealth.
        trueHealth -= Mathf.Clamp(newDamage, 0, stats.maxHealth);
        trueHealth = Mathf.Clamp(trueHealth, 0, stats.maxHealth);
        animator.SetTrigger("Damage");
    }

    public void Attack(Character target) {
        if (!isDead) {
            newTarget = target;
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }
    public void Skill() {
        if (!isDead) {
            if (stats.name == "Knight" && skillUse >= 0) {
                temporaryHealth += stats.energy;
                stats.power += temporaryHealth;
                skillUse -= 3;
            }

            if (stats.name == "Ghost" && skillUse >= 0) {
                trueHealth += stats.energy;
                skillUse -= 3;
            }

            if (stats.name == "Bat") {
                Attack(playerChar);
                if (playerChar.stats.power >= 1) {
                    playerChar.stats.power -= 1;
                    skillUse -= 4;
                }
            }

            if (stats.name == "Rabbit") {
                stats.power += stats.energy + 2;
                skillUse -= 5;
            }

            animator.SetTrigger("Skill");
        }
    }
    public void Defense() {
        if (!isDead) {
            temporaryHealth += stats.energy * 3;
            animator.SetTrigger("Defense");
        }
    }
    void Death() {
        isDead = true;
        if (GetComponent<CapsuleCollider>() != null) {
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            col.enabled = false;
        }
        animator.SetTrigger("Death");
    }

    public void OnRaycastHit() {
        spotlight.enabled = true;
    }
    public void OnRaycastEnd() {
        spotlight.enabled = false;
    }
    public void OnTurnStart() {
        temporaryHealth = 0;
        skillUse += 1;
    }
}