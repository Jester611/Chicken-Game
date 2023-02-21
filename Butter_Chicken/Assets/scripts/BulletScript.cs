using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // ## VARS ##

    // these values will come from a singleton to handle upgrades

    [SerializeField] private float size;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float knockback;
    [SerializeField] private float damage;

    private void Start() {
        size = GlobalStats.instance.bulletSize;
        explosionRadius = GlobalStats.instance.bulletExplosiveRadius;
        knockback = GlobalStats.instance.bulletKnockback;
        damage = GlobalStats.instance.bulletDamage;
        transform.localScale = new Vector3(size, size, size);
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision other) {
        GameObject hit = other.gameObject;
        IDamagable damagable = hit.GetComponent<IDamagable>();

        // ## ON HIT DAMAGE + KNOCKBACK ##
        if (damagable != null) {
            // Take damage
            damagable.TakeDamage(damage);

            // Knockback
            Vector3 direction = gameObject.GetComponent<Rigidbody>().velocity.normalized;
            damagable.gameObject.GetComponent<Rigidbody>().AddForce(direction * knockback, ForceMode.Impulse);
        }

        // ## EXPLOSIVE KNOCKBACK + DAMAGE ##
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in affectedObjects) {
            damagable = collider.gameObject.GetComponent<IDamagable>();

            if (damagable != null && hit != collider.gameObject) {
                // damage and knockback fall off with distance
                float distance = Vector3.Distance(transform.position, damagable.rb.position);
                float powerFalloff = (explosionRadius - distance) / explosionRadius;

                // Take Damage
                damagable.TakeDamage(powerFalloff * damage);

                // Knockback
                Vector3 direction = damagable.rb.position - transform.position;

                damagable.rb.AddForce(
                    direction.normalized * powerFalloff * knockback,
                    ForceMode.Impulse
                );
            }
        }

        Destroy(gameObject);
    }
}
