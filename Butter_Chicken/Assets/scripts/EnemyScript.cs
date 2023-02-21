using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public int enemykill;

    [SerializeField] Transform player;
    float maxHP;
    float movementSpeed;
    [SerializeField] Rigidbody rb;
    private float currentHP;
    private float distance;
    Vector3 direction;

    public static event Action OnEnemyKilled;
    
   

    
    private void Awake() {
        movementSpeed = GlobalStats._instance.enemySpeed;
        maxHP = GlobalStats._instance.enemyHP;
        currentHP = maxHP;
    }

    public void TakeDamage(float damageTaken){
        currentHP -= damageTaken;
    }
    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }
    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
        
    }

    void Update()
    {
    
    if (currentHP <= 0){
            Die();
           
        }

    if(currentHP <= 0)
    {
        enemykill = enemykill +1;
    }
    }



    private void FixedUpdate(){

        distance = Vector3.Distance(transform.position, player.position);
        direction = player.position - transform.position;
        direction.Normalize();
        
        rb.AddForce(direction*movementSpeed, ForceMode.VelocityChange);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }
    private void Die(){
        Destroy(gameObject);
        OnEnemyKilled?.Invoke();
    }

    private void UpdateStats(){
        movementSpeed = GlobalStats._instance.enemySpeed;
        maxHP = GlobalStats._instance.enemyHP;
    }
}
