using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    PlayerController player;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    
    private float bulletSpeed;
    private float reloadTime;
    bool readyToFire;

    private void Awake() {
        UpdateStats();
        Reload();
    }

    private void Start() {
        player = PlayerController.instance;
    }

    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }

    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
    }

    public void Fire() {
        if(readyToFire){
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            //Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            bullet.GetComponent<Rigidbody>().AddForce(gunPoint.forward * bulletSpeed, ForceMode.Impulse);
            readyToFire = false;
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload() {
        readyToFire = true;
    }

    private void UpdateStats() {
        bulletSpeed = GlobalStats.instance.gunBulletSpeed;
        reloadTime = GlobalStats.instance.gunRateOfFire;
    }
}
