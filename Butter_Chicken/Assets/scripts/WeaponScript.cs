using UnityEngine;
using UnityEngine.Audio;

public class WeaponScript : MonoBehaviour
{
    PlayerController player;
    Rigidbody rb;
    AudioSource sound;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;

    bool readyToFire;
        
    // ## UPGRADEABLE VARS ##
    private float bulletSpeed;
    private float reloadTime;
    private float recoil;
    private float spread;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }

    private void Start() {
        player = PlayerController.instance;
        UpdateStats();
        Reload();
    }

    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }

    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
    }

    public void Fire() {
        if(readyToFire){
            // BULLET SPREAD
            Quaternion spreadRotation = gunPoint.rotation;
            if(spread > 0){
                spreadRotation = gunPoint.rotation*Quaternion.Euler(0,Random.Range(-spread/2, spread/2), 0);
            }
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, spreadRotation);

            // in case we'd want to add player velocity to projectile speed
            // Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            // then
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed + playerVelocity, ForceMode.VelocityChange);
            // this didn't look very good in gameplay though

            // SPAWN BULLET
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
            readyToFire = false;
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
            // GUN RECOIL
            rb.AddForce(-gunPoint.forward*recoil, ForceMode.Impulse);
            // RELOAD TIMER
            sound.PlayOneShot(gunshotSound);
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload() {
        readyToFire = true;
    }

    private void UpdateStats() {
        if(GameManager.instance != null){
            bulletSpeed = GameManager.instance.gunBulletSpeed;
            reloadTime = GameManager.instance.gunRateOfFire;
            recoil = GameManager.instance.gunRecoil;
            spread = GameManager.instance.gunSpread;
        }
        else{Debug.Log("weapon not detecting singleton ffs");}
    }
}
