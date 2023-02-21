using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    [SerializeField] private LayerMask aimMask;
    [SerializeField] private float movementSpeed;

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void Update() {
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
            // gameObject.SendMessage("Fire");
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector3(moveDirection.x * movementSpeed, 0f, moveDirection.y * movementSpeed), ForceMode.VelocityChange);
    }

}
