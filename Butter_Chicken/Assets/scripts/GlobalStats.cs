using System;
using UnityEngine;

public class GlobalStats : MonoBehaviour
{
    public static GlobalStats _instance { get; private set; }

    public static event Action OnPlayerLevel;


    // ## VARIABLES RELATED TO UPGRADES ##
    public float bulletDamage;
    public float bulletSize;
    public float bulletExplosiveRadius;
    public float bulletKnockback;
    public float gunRateOfFire;
    public float gunBulletSpeed;
    public float playerSpeed;
    public float enemyHP;
    public float enemySpeed;
    public float enemySize;

    // Player stats
    private int playerXP;
    public int playerLevelRequirement;
    private void Start() {
        if (_instance != null && _instance != this) { 
            Destroy(this); 
        } 
        else { 
            _instance = this; 
        } 
    }
    private void Awake() 
    { 
        if (_instance != null && _instance != this) { 
            Destroy(this); 
        } 
        else { 
            _instance = this; 
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
        if (playerXP >= playerLevelRequirement){
            OnPlayerLevel?.Invoke();
        }
    }
}
