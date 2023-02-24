using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    // ## VARS ##
    private float explosionRadius;
    private float knockback;
    private float damage;

    private void Start() {
        Destroy(gameObject, 10f);
        GetStats();
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
        if(explosionRadius > 0)
        // this hopefully will prevent infinite force crash
        {        
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
            GameObject explosionFX = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosionFX.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
            Destroy(explosionFX, 1f);
        }
        Destroy(gameObject);
    }
    private void GetStats(){
        explosionRadius = GameManager.instance.bulletExplosiveRadius;
        knockback = GameManager.instance.bulletKnockback;
        damage = GameManager.instance.bulletDamage;
    }
}
