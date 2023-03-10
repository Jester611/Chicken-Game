using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour

{   
    // ## ESSENTIALS ##
    public static PlayerController instance;
    [HideInInspector] public Rigidbody rb {get; set;}
    private Camera cam;
    private WeaponScript weapon;
    private Animator animator;
    [SerializeField] private LayerMask aimMask;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    float gracePeriod; // brief invincibility after taking damage
    public Vector3 lookPoint;

    // ## PLAYER STATS ##
    public float maxHealth {get; set;}
    public float currentHealth {get; set;} 
    private float movementSpeed;
    float invincibilityDuration;

    public static event Action OnPlayerDeath;
    public event Action OnDamaged;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<WeaponScript>();
        UpdateStats();
    }
    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }
    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
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
            Physics.SphereCast(aimRay,0.61f, out hit, Mathf.Infinity, aimMask);
            lookPoint = new Vector3(
            hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookPoint);

            if (Input.GetMouseButton(0)) {
                weapon.Fire();
            }
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.Impulse);
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
            animator.Play("PlayerDamaged");
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
        }
        else{Debug.Log("player not detecting singleton ffs");}
    }

    private void PlayerDies(){
        OnPlayerDeath?.Invoke();
    }
}
