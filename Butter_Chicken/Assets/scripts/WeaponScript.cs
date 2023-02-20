using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{   


    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    bool readyToFire;

    private void Awake() {
        //bulletSpeed = GlobalStats._instance.gunBulletSpeed;
        //reloadTime = GlobalStats._instance.gunRateOfFire;
        Reload();
    }

    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }
    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
        
    }
    public void Fire(){
        if(readyToFire){
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            //Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            bullet.GetComponent<Rigidbody>().AddForce(gunPoint.forward*bulletSpeed, ForceMode.Impulse);
            readyToFire = false;
            
            Invoke("Reload", reloadTime);
        }
    }
    private void Reload(){
        readyToFire = true;
    }

    private void UpdateStats(){
        bulletSpeed = GlobalStats._instance.gunBulletSpeed;
        reloadTime = GlobalStats._instance.gunRateOfFire;
    }
}
