using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour

{   
    // ## ESSENTIALS ##
    public static PlayerController instance;
    [HideInInspector] public Rigidbody rb {get; set;}

    Camera cam;
    WeaponScript weapon;
    [SerializeField] private LayerMask aimMask;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    float gracePeriod; // brief invincibility after taking damage


    // ## PLAYER STATS ##
    public float maxHealth {get; set;}
    public float currentHealth {get; set;} 
    private float movementSpeed;
    float invincibilityDuration;

    public static event Action OnPlayerDeath;
    public event Action OnDamaged;
    public event Action OnDeath;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        OnDeath += () => {OnPlayerDeath?.Invoke();};
    }

    private void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<WeaponScript>();
        UpdateStats();
    }

    private void Update() {
        if (!GameManager.isPaused){
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
        if (gracePeriod <= 0){
            currentHealth -= damage;
            OnDamaged?.Invoke();
            if (currentHealth <= 0){
                PlayerDies();
            }
            gracePeriod = invincibilityDuration;
            GameManager.instance.UpdateHealthBar();
        }
    }

    private void UpdateStats() {
        if(GameManager.instance != null){
            movementSpeed = GameManager.instance.playerSpeed;
            maxHealth = GameManager.instance.playerMaxHP;
            invincibilityDuration = GameManager.instance.playerInvincibilityTimer;
            rb.mass = GameManager.instance.playerWeight;
            rb.drag = GameManager.instance.playerDrag;
            currentHealth = maxHealth; //full heal on level
            Debug.Log($"stats updated {currentHealth} HP, {movementSpeed} movementSpeed, {invincibilityDuration} invincibilityTimer");
        }
        else{Debug.Log("player not detecting singleton ffs");}
    }

    private void PlayerDies(){
        OnDeath?.Invoke();
        OnPlayerDeath?.Invoke();
    }
}
