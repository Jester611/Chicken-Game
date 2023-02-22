using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamagable
{   
    // ## ESSENTIALS ##
    public static PlayerController instance;
    public static event Action OnPlayerDeath;


    [HideInInspector] public Rigidbody rb {get; set;}
    Camera cam;
    WeaponScript weapon;
    [SerializeField] private LayerMask aimMask;
    private Vector2 moveDirection;
    private Vector2 mousePosition;

    // ## PLAYER STATS ##
    private float movementSpeed;
    private float maxHP;
    float currentHP;
    float gracePeriod;
    float invincibilityTimer; //for grace period when taking damage

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        UpdateStats();
    }

    private void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<WeaponScript>();
    }

    private void Update() {
        if (!UIScript.isPaused){
            
            if(invincibilityTimer > 0){
                invincibilityTimer -= Time.deltaTime;
            }
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            //handle diagonal movement in isometric envoroniment
            moveDirection = new Vector2(moveY + moveX, moveY - moveX).normalized;

            RaycastHit hit;
            Ray aimRay = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(aimRay, out hit, Mathf.Infinity, aimMask);

            transform.LookAt(new Vector3(
            hit.point.x, transform.position.y, hit.point.z));

            if (Input.GetMouseButton(0)) {
                weapon.Fire();
            }
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.VelocityChange);
    }

    public void TakeDamage(float damage){
        if (invincibilityTimer <= 0){
            currentHP -= damage;
            Debug.Log($"Player takes {damage} damage");
            if (currentHP <= 0){
                PlayerDies();
            }
            invincibilityTimer = gracePeriod;
        }
    }

    private void UpdateStats() {
        if(GlobalStats.instance != null){
            movementSpeed = GlobalStats.instance.playerSpeed;
            maxHP = GlobalStats.instance.playerMaxHP;
            invincibilityTimer = GlobalStats.instance.playerInvincibilityTimer;
            currentHP = maxHP; //full heal on level
            Debug.Log($"stats updated {currentHP} HP, {movementSpeed} movementSpeed, {invincibilityTimer} invincibilityTimer");
            
        }
        else{Debug.Log("player not detecting singleton ffs");}
    }

    private void PlayerDies(){
        OnPlayerDeath?.Invoke();
    }
}
