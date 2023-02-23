using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header(header: "Upgrade name")]
    public string upgradeName;
    public string description;
    [Header(header: "ADD VALUES")]
    [Header("Bullet")]
    public float add_bulletDamage = 0f;
    public float add_bulletExplosiveRadius = 0f;
    public float add_bulletKnockback = 0f;
    [Header("Weapon")]
    public float add_gunRateOfFire = 0f;
    public float add_gunBulletSpeed = 0f;
    public float add_gunRecoil = 0f;
     public float add_gunSpread = 0f;

    [Header("Player")]
    public float add_playerSpeed = 0f;
    public float add_playerMaxHP = 0f;
    public float add_playerInvincibilityTimer = 0f;
    public float add_playerWeight = 0f;
    public float add_playerDrag = 0f;

    [Header("Enemy")]
    public float add_enemyHP = 0f;
    public float add_enemySpeed = 0f;
    public float add_enemySize = 0f;
    public float add_enemyAttack = 0f;
    public float add_enemyWeight = 0f;

    [Header(header: "MULTIPLY VALUES")]
    [Header("Bullet")]
    public float mult_bulletDamage = 1f;
    public float mult_bulletExplosiveRadius = 1f;
    public float mult_bulletKnockback = 1f;
    [Header("Weapon")]
    public float mult_gunRateOfFire = 1f;
    public float mult_gunBulletSpeed = 1f;
    public float mult_gunRecoil = 1f;
     public float mult_gunSpread = 1f;

    [Header("Player")]
    public float mult_playerSpeed = 1f;
    public float mult_playerMaxHP = 1f;
    public float mult_playerInvincibilityTimer = 1f;
    public float mult_playerWeight = 1f;
    public float mult_playerDrag = 1f;

    [Header("Enemy")]
    public float mult_enemyHP = 1f;
    public float mult_enemySpeed = 1f;
    public float mult_enemySize = 1f;
    public float mult_enemyAttack = 1f;
    public float mult_enemyWeight = 1f;

    public void Apply(){
        Add();
        Multiply();
    }
    private void Add(){
        GameManager.instance.bulletDamage += add_bulletDamage;
        GameManager.instance.bulletExplosiveRadius += add_bulletExplosiveRadius;
        GameManager.instance.bulletKnockback += add_bulletKnockback;

        GameManager.instance.gunRateOfFire += add_gunRateOfFire;
        GameManager.instance.gunBulletSpeed += add_gunBulletSpeed;
        GameManager.instance.gunRecoil += add_gunRecoil;
        GameManager.instance.gunSpread += add_gunSpread;

        GameManager.instance.playerSpeed += add_playerSpeed;
        GameManager.instance.playerMaxHP += add_playerMaxHP;
        GameManager.instance.playerInvincibilityTimer += add_playerInvincibilityTimer;
        GameManager.instance.playerWeight += add_playerWeight;
        GameManager.instance.playerDrag += add_playerDrag;

        GameManager.instance.enemyHP += add_enemyHP;
        GameManager.instance.enemySpeed += add_enemySpeed;
        GameManager.instance.enemySize += add_enemySize;
        GameManager.instance.enemyAttack += add_enemyAttack;
        GameManager.instance.enemyWeight += add_enemyWeight;
    }
    private void Multiply(){
        GameManager.instance.bulletDamage *= mult_bulletDamage;
        GameManager.instance.bulletExplosiveRadius *= mult_bulletExplosiveRadius;
        GameManager.instance.bulletKnockback *= mult_bulletKnockback;

        GameManager.instance.gunRateOfFire *= mult_gunRateOfFire;
        GameManager.instance.gunBulletSpeed *= mult_gunBulletSpeed;
        GameManager.instance.gunRecoil *= mult_gunRecoil;
        GameManager.instance.gunSpread *= mult_gunSpread;

        GameManager.instance.playerSpeed *= mult_playerSpeed;
        GameManager.instance.playerMaxHP *= mult_playerMaxHP;
        GameManager.instance.playerInvincibilityTimer *= mult_playerInvincibilityTimer;
        GameManager.instance.playerWeight *= mult_playerWeight;
        GameManager.instance.playerDrag *= mult_playerDrag;

        GameManager.instance.enemyHP *= mult_enemyHP;
        GameManager.instance.enemySpeed *= mult_enemySpeed;
        GameManager.instance.enemySize *= mult_enemySize;
        GameManager.instance.enemyAttack *= mult_enemyAttack;
        GameManager.instance.enemyWeight *= mult_enemyWeight;
    }
}