using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // ## VARS ##

    // these values will come from a singleton to handle upgrades

    [SerializeField] private float size; 
    [SerializeField] private float explosionRadius;
    [SerializeField] private float knockback;
    [SerializeField] private float damage;
    private void Awake(){
        size = GlobalStats._instance.bulletSize;
        explosionRadius = GlobalStats._instance.bulletExplosiveRadius;
        knockback = GlobalStats._instance.bulletKnockback;
        damage = GlobalStats._instance.bulletDamage;
        transform.localScale = new Vector3(size, size, size); 
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision other) {
    
    GameObject hit = other.gameObject;
    Rigidbody enemyRB;

    // ## ON HIT DAMAGE + KNOCKBACK ##
    if (hit.CompareTag("Enemy")){
        // Take damage
        hit.GetComponent<EnemyScript>().TakeDamage(damage);

        // Knockback
        enemyRB = hit.GetComponent<Rigidbody>();
        Vector3 direction = gameObject.GetComponent<Rigidbody>().velocity.normalized;
        enemyRB.AddForce(direction*knockback, ForceMode.Impulse);
    }

    // ## EXPLOSIVE KNOCKBACK + DAMAGE ##
    Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider collider in affectedObjects){
            GameObject affected = collider.gameObject;

            if(affected.CompareTag("Enemy") && hit != affected){
             
                enemyRB = collider.GetComponent<Rigidbody>();
                
                if(enemyRB != null){

                    Vector3 direction;
                    float powerFalloff;
                    float distance;

                    // damage and knockback fall off with distance
                    distance = Vector3.Distance(transform.position,affected.transform.position);
                    powerFalloff = (explosionRadius - distance) / explosionRadius;

                    // Take Damage
                    hit.GetComponent<EnemyScript>().TakeDamage(powerFalloff * damage);
                    
                    // Knockback
                    direction = affected.transform.position - transform.position;
                    Debug.Log(direction);
                    enemyRB.AddForce(direction.normalized * powerFalloff * knockback, ForceMode.Impulse);
                }
                else{Debug.Log("rigidbody null");}
            }
        }

        Destroy(gameObject);
    }
}
