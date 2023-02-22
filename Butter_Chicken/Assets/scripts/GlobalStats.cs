using System;
using UnityEngine;

public class GlobalStats : MonoBehaviour
{
    public static GlobalStats instance { get; private set; }

    public static event Action OnPlayerLevel;

    //this is a terrible solution and is probably better reworked
    //yknowwhat whatever

    // ## VARIABLES RELATED TO UPGRADES ##
    [Header("Bullet")]
    public float bulletDamage;
    public float bulletSize;
    public float bulletExplosiveRadius;
    public float bulletKnockback;
    [Header("Weapon")]
    public float gunRateOfFire;
    public float gunBulletSpeed;
    public float gunRecoil;

    [Header("Player")]
    public float playerSpeed;
    public float playerMaxHP;
    public int playerLevelRequirement;
    public float playerInvincibilityTimer;
    [Header("Enemy")]
    public float enemyHP;
    public float enemySpeed;
    public float enemySize;
    public float enemyAttack;

    // Player stats
    private int playerXP;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    private void OnEnable() {
        EnemyScript.OnEnemyKilled += GainXP;
    }

    private void OnDisable() {
        EnemyScript.OnEnemyKilled -= GainXP;
    }

    private void GainXP() {
        playerXP++;
        if (playerXP >= playerLevelRequirement)
        {
            OnPlayerLevel?.Invoke();
        }
    }
}
