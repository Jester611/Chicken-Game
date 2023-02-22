using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WeaponScript))]
public class PlayerController : MonoBehaviour, IDamagable
{
    public static PlayerController instance;
    [HideInInspector] public Rigidbody rb {get; set;}
    Camera cam;
    WeaponScript weapon;

    [SerializeField] private LayerMask aimMask;
    [SerializeField] private float movementSpeed;
    public float maxHealth {get; set;}
    public float currentHealth {get; set;}

    private Vector2 moveDirection;
    private Vector2 mousePosition;

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
    }

    private void Update() {
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

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.VelocityChange);
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        OnDamaged?.Invoke();
        if(currentHealth <= 0){
            OnDeath?.Invoke();
        }
    }
}
