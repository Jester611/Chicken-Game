using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamagable
{   
    // ## ESSENTIALS ##
    public static PlayerController instance;
    [HideInInspector] public Rigidbody rb {get; set;}
    Camera cam;
    WeaponScript weapon;
    [SerializeField] private LayerMask aimMask;
    private Vector2 moveDirection;
    private Vector2 mousePosition;

    // ## PLAYER STATS ##
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxHP;
    float currentHP;

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
        //not implemented
    }

    private void UpdateStats() {
        if(GlobalStats.instance != null){
            movementSpeed = GlobalStats.instance.playerSpeed;
            maxHP = GlobalStats.instance.playerMaxHP;
            currentHP = maxHP; //full heal on level
        }
        else{Debug.Log("not detecting singleton ffs");}
    }
}
