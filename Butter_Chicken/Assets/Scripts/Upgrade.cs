using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header(header: "Upgrade data")]
    public string upgradeName;
    public string description;
    public string secondDescription;
    [Tooltip("1 = weapon,  2 = player mod, 3 = enemy mod")]
    public int upgradeType;

    [Header(header: "MODIFIED VALUES")]
    [Header("Bullet")]
    public float bulletDamage = 5f;
    public float bulletExplosiveRadius = 0f;
    public float bulletKnockback = 0f;
    [Header("Weapon")]
    public float gunRateOfFire = 0.7f;
    public float gunBulletSpeed = 15f;
    public float gunRecoil = 0f;
    public float gunSpread = 0f;
    public int gunBurstSize = 1;


    [Header("Player")]
    public float playerSpeed = 1f;
    public float playerMaxHP = 50f;
    public float playerInvincibilityTimer = 0.4f;
    public float playerWeight = 1f;
    public float playerDrag = 8f;

    [Header("Enemy")]
    public float enemyHP = 10f;
    public float enemySpeed = 0.05f;
    public float enemySize = 1f;
    public float enemyAttack = 8f;
    public float enemyWeight = 0.2f;


    public void Apply(){
        switch (upgradeType){
            case 1:{
                GameManager.instance.bulletDamage = bulletDamage;
                GameManager.instance.bulletExplosiveRadius = bulletExplosiveRadius;
                GameManager.instance.bulletKnockback = bulletKnockback;

                GameManager.instance.gunRateOfFire = gunRateOfFire;
                GameManager.instance.gunBulletSpeed = gunBulletSpeed;
                GameManager.instance.gunRecoil = gunRecoil;
                GameManager.instance.gunSpread = gunSpread;
                GameManager.instance.gunBurstSize = gunBurstSize;
                break;
            }
            case 2:{
                GameManager.instance.playerSpeed = playerSpeed;
                GameManager.instance.playerMaxHP = playerMaxHP;
                GameManager.instance.playerInvincibilityTimer = playerInvincibilityTimer;
                GameManager.instance.playerWeight = playerWeight;
                GameManager.instance.playerDrag = playerDrag;
                break;
            }
            case 3:{
                GameManager.instance.enemyHP = enemyHP;
                GameManager.instance.enemySpeed = enemySpeed;
                GameManager.instance.enemySize = enemySize;
                GameManager.instance.enemyAttack = enemyAttack;
                GameManager.instance.enemyWeight = enemyWeight;
                break;
            }
            case 0:{
                Debug.Log("unassigned upgrade class applied, fix it fucko");
                GameManager.instance.bulletDamage = bulletDamage;
                GameManager.instance.bulletExplosiveRadius = bulletExplosiveRadius;
                GameManager.instance.bulletKnockback = bulletKnockback;

                GameManager.instance.gunRateOfFire = gunRateOfFire;
                GameManager.instance.gunBulletSpeed = gunBulletSpeed;
                GameManager.instance.gunRecoil = gunRecoil;
                GameManager.instance.gunSpread = gunSpread;

                GameManager.instance.playerSpeed = playerSpeed;
                GameManager.instance.playerMaxHP = playerMaxHP;
                GameManager.instance.playerInvincibilityTimer = playerInvincibilityTimer;
                GameManager.instance.playerWeight = playerWeight;
                GameManager.instance.playerDrag = playerDrag;

                GameManager.instance.enemyHP = enemyHP;
                GameManager.instance.enemySpeed = enemySpeed;
                GameManager.instance.enemySize = enemySize;
                GameManager.instance.enemyAttack = enemyAttack;
                GameManager.instance.enemyWeight = enemyWeight;
                break;
            }
            default:
                break;
        }
    }
}