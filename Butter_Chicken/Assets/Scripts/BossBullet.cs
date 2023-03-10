using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    [SerializeField] private float knockback = 2f;
    [SerializeField] private float bulletSpeed = 10f;

    Rigidbody rb;
    private void Start() {
        Destroy(gameObject, 10f);
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other) {
        GameObject hit = other.gameObject;
        PlayerController player = hit.GetComponent<PlayerController>();
        if (player != null) {

            player.TakeDamage(damage);

            player.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * knockback, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
