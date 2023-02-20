using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ## VARIABLES ##
    [SerializeField] private GameObject weapon;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private float movementSpeed;

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    // INPUTS

    void Awake(){

    }
    private void Update(){

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        //handle diagonal movement in isometric envoroniment
        moveDirection = new Vector2(moveY+moveX, moveY-moveX).normalized;

        RaycastHit hit;
        Ray aimRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(aimRay, out hit, Mathf.Infinity, aimMask);

        transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));


        if(Input.GetMouseButton(0)){
            weapon.SendMessage("Fire");
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.VelocityChange);
    }
}
