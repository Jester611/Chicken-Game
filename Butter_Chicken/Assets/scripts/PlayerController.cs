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
    float gracePeriod; // brief invincibility after taking damage


    // ## PLAYER STATS ##
    private float movementSpeed;
    private float maxHP;
    [HideInInspector] public float currentHP;
    float invincibilityDuration;

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
            currentHP -= damage;
            if (currentHP <= 0){
                PlayerDies();
            }
            gracePeriod = invincibilityDuration;
            UIScript.instance.UpdateHealthBar();
        }
    }

    private void UpdateStats() {
        if(UIScript.instance != null){
            movementSpeed = UIScript.instance.playerSpeed;
            maxHP = UIScript.instance.playerMaxHP;
            invincibilityDuration = UIScript.instance.playerInvincibilityTimer;
            rb.mass = UIScript.instance.playerWeight;
            rb.drag = UIScript.instance.playerDrag;
            currentHP = maxHP; //full heal on level
            Debug.Log($"stats updated {currentHP} HP, {movementSpeed} movementSpeed, {invincibilityDuration} invincibilityTimer");
            
        }
        else{Debug.Log("player not detecting singleton ffs");}
    }

    private void PlayerDies(){
        OnPlayerDeath?.Invoke();
    }
}
