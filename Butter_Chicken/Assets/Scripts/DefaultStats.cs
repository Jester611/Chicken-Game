using UnityEngine;

[CreateAssetMenu(menuName = "StatSheet/Defaults")]
public class DefaultStats : ScriptableObject
{
    // ## VARIABLES RELATED TO UPGRADES ##
    [Header("Bullet")]
    public float bulletDamage = 5;
    public float bulletExplosiveRadius = 0;
    public float bulletKnockback = 0;
    [Header("Weapon")]
    public float gunRateOfFire = 0.7f;
    public float gunBulletSpeed = 15f;
    public float gunRecoil = 0f;
    public float gunSpread = 0f;

    [Header("Player")]
    public float playerSpeed = 1f;
    public float playerMaxHP = 50f;
    public float playerInvincibilityTimer = 0.4f;
    public float playerWeight = 1f;
    public float playerDrag = 8f;

    [Header("Enemy")]
    public float enemyHP = 10f;
    public float enemySpeed = 0.02f;
    public float enemySize = 1f;
    public float enemyAttack = 8f;
    public float enemyWeight = 0.2f;

}
