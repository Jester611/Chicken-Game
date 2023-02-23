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
        UIScript.instance.bulletDamage += add_bulletDamage;
        UIScript.instance.bulletExplosiveRadius += add_bulletExplosiveRadius;
        UIScript.instance.bulletKnockback += add_bulletKnockback;

        UIScript.instance.gunRateOfFire += add_gunRateOfFire;
        UIScript.instance.gunBulletSpeed += add_gunBulletSpeed;
        UIScript.instance.gunRecoil += add_gunRecoil;
        UIScript.instance.gunSpread += add_gunSpread;

        UIScript.instance.playerSpeed += add_playerSpeed;
        UIScript.instance.playerMaxHP += add_playerMaxHP;
        UIScript.instance.playerInvincibilityTimer += add_playerInvincibilityTimer;
        UIScript.instance.playerWeight += add_playerWeight;
        UIScript.instance.playerDrag += add_playerDrag;

        UIScript.instance.enemyHP += add_enemyHP;
        UIScript.instance.enemySpeed += add_enemySpeed;
        UIScript.instance.enemySize += add_enemySize;
        UIScript.instance.enemyAttack += add_enemyAttack;
        UIScript.instance.enemyWeight += add_enemyWeight;
    }
    private void Multiply(){
        UIScript.instance.bulletDamage *= mult_bulletDamage;
        UIScript.instance.bulletExplosiveRadius *= mult_bulletExplosiveRadius;
        UIScript.instance.bulletKnockback *= mult_bulletKnockback;

        UIScript.instance.gunRateOfFire *= mult_gunRateOfFire;
        UIScript.instance.gunBulletSpeed *= mult_gunBulletSpeed;
        UIScript.instance.gunRecoil *= mult_gunRecoil;
        UIScript.instance.gunSpread *= mult_gunSpread;

        UIScript.instance.playerSpeed *= mult_playerSpeed;
        UIScript.instance.playerMaxHP *= mult_playerMaxHP;
        UIScript.instance.playerInvincibilityTimer *= mult_playerInvincibilityTimer;
        UIScript.instance.playerWeight *= mult_playerWeight;
        UIScript.instance.playerDrag *= mult_playerDrag;

        UIScript.instance.enemyHP *= mult_enemyHP;
        UIScript.instance.enemySpeed *= mult_enemySpeed;
        UIScript.instance.enemySize *= mult_enemySize;
        UIScript.instance.enemyAttack *= mult_enemyAttack;
        UIScript.instance.enemyWeight *= mult_enemyWeight;
    }
}