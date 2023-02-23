using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WeaponScript))]
public class PlayerController : MonoBehaviour, IDamagable
{   
    // ## ESSENTIALS ##
    public static PlayerController instance;
    Camera cam;
    WeaponScript weapon;
    [HideInInspector] public Rigidbody rb {get; set;}
    [SerializeField] private LayerMask aimMask;

    public float maxHealth {get; set;}
    public float currentHealth {get; set;}

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    float gracePeriod; // brief invincibility after taking damage


    private float movementSpeed;
    float invincibilityDuration;

    public event Action OnDamaged;
    public event Action OnDeath;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }


    private void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<WeaponScript>();
        UpdateStats();
    }

    private void Update() {
        if (!UIScript.isPaused){
            if(gracePeriod > 0){
                gracePeriod -= Time.deltaTime;
            }
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            //handle diagonal movement in isometric envoroniment
            moveDirection = new Vector2(moveY + moveX, moveY - moveX).normalized;

            RaycastHit hit;
            Ray aimRay = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(aimRay, out hit, Mathf.Infinity, aimMask);

            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

            if (Input.GetMouseButton(0)) {
                weapon.Fire();
            }
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.VelocityChange);
    }

    public void TakeDamage(float damage){
        if (gracePeriod <= 0){
            currentHealth -= damage;
            OnDamaged?.Invoke();
            if (currentHealth <= 0){
                OnDeath?.Invoke();
            }
            gracePeriod = invincibilityDuration;
            UIScript.instance.UpdateHealthBar();
        }
    }

    private void UpdateStats() {
        if(UIScript.instance != null){
            movementSpeed = UIScript.instance.playerSpeed;
            maxHealth = UIScript.instance.playerMaxHP;
            invincibilityDuration = UIScript.instance.playerInvincibilityTimer;
            currentHealth = maxHealth; //full heal on level
            Debug.Log($"stats updated {currentHealth} HP, {movementSpeed} movementSpeed, {invincibilityDuration} invincibilityTimer");
            
        }
        else{Debug.Log("player not detecting singleton ffs");}
    }
}
