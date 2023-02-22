using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    PlayerController player;
    Rigidbody rb;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    
    private float bulletSpeed;
    private float reloadTime;
    private float recoil;
    bool readyToFire;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
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

            // in case we'd want to add player velocity to projectile speed
            // Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            // this didn't look very good in gameplay though

            bullet.GetComponent<Rigidbody>().AddForce(gunPoint.forward * bulletSpeed, ForceMode.VelocityChange);
            readyToFire = false;
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

            // gun recoil
            rb.AddForce(-gunPoint.forward*recoil, ForceMode.Impulse);

            Invoke("Reload", reloadTime);
        }
    }

    private void Reload() {
        readyToFire = true;
    }

    private void UpdateStats() {
        if(GlobalStats.instance != null){
            bulletSpeed = GlobalStats.instance.gunBulletSpeed;
            reloadTime = GlobalStats.instance.gunRateOfFire;
            recoil = GlobalStats.instance.gunRecoil;
        }
        else{Debug.Log("weapon not detecting singleton ffs");}
    }
}
