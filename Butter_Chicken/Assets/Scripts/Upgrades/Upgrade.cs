using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header(header: "Upgrade name")]
    public string upgradeName;
    public string description;
    public string secondDescription;

    [Header(header: "ADD VALUES")]
    [Header("Bullet")]
    public float add_bulletDamage = 5f;
    public float add_bulletExplosiveRadius = 0f;
    public float add_bulletKnockback = 0f;
    [Header("Weapon")]
    public float add_gunRateOfFire = 0.7f;
    public float add_gunBulletSpeed = 15f;
    public float add_gunRecoil = 0f;
     public float add_gunSpread = 0f;

    [Header("Player")]
    public float add_playerSpeed = 1f;
    public float add_playerMaxHP = 50f;
    public float add_playerInvincibilityTimer = 0.4f;
    public float add_playerWeight = 1f;
    public float add_playerDrag = 8f;

    [Header("Enemy")]
    public float add_enemyHP = 10f;
    public float add_enemySpeed = 0.02f;
    public float add_enemySize = 1f;
    public float add_enemyAttack = 8f;
    public float add_enemyWeight = 0.2f;


    public void Apply(){
        GameManager.instance.bulletDamage = add_bulletDamage;
        GameManager.instance.bulletExplosiveRadius = add_bulletExplosiveRadius;
        GameManager.instance.bulletKnockback = add_bulletKnockback;

        GameManager.instance.gunRateOfFire = add_gunRateOfFire;
        GameManager.instance.gunBulletSpeed = add_gunBulletSpeed;
        GameManager.instance.gunRecoil = add_gunRecoil;
        GameManager.instance.gunSpread = add_gunSpread;

        GameManager.instance.playerSpeed = add_playerSpeed;
        GameManager.instance.playerMaxHP = add_playerMaxHP;
        GameManager.instance.playerInvincibilityTimer = add_playerInvincibilityTimer;
        GameManager.instance.playerWeight = add_playerWeight;
        GameManager.instance.playerDrag = add_playerDrag;

        GameManager.instance.enemyHP = add_enemyHP;
        GameManager.instance.enemySpeed = add_enemySpeed;
        GameManager.instance.enemySize = add_enemySize;
        GameManager.instance.enemyAttack = add_enemyAttack;
        GameManager.instance.enemyWeight = add_enemyWeight;
    }
}