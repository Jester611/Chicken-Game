using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    PlayerController player;
    Rigidbody rb;
    AudioSource sound;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;

    private Vector3 aimPoint;
    bool readyToFire;
        
    // ## UPGRADEABLE VARS ##
    private float bulletSpeed;
    private float reloadTime;
    private float recoil;
    private float spread;
    private int burstSize;

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

    public void Fire() {    //yeah this could be more efficient using object pooling idc
        if(readyToFire){

            // in case we'd want to add player velocity to projectile speed
            // Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            // then
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed + playerVelocity, ForceMode.VelocityChange);
            // this didn't look very good in gameplay though
            // a good looking middle ground could be achieved using lerp
            aimPoint = new Vector3(player.lookPoint.x, gunPoint.position.y, player.lookPoint.z);
            gunPoint.LookAt(aimPoint);
            // burst fire
            for(int i = 0; i < burstSize; i++){
                Quaternion spreadRotation = gunPoint.rotation;
                if(spread > 0){
                    spreadRotation = gunPoint.rotation*Quaternion.Euler(0,Random.Range(-spread/2, spread/2), 0);
                }

                // SPAWN BULLET
                GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, spreadRotation);

                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);

                 Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
            }
            readyToFire = false;
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
            burstSize = GameManager.instance.gunBurstSize;
        }
        else{Debug.Log("weapon not detecting singleton ffs");}
    }
}
